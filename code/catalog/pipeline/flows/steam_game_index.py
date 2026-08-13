import logging
from datetime import datetime, timezone

import niquests
from prefect import flow, task
from prefect.concurrency.sync import concurrency
from utilities.database import get_connection
from flows.igdb import dlt_igdb_default
from utilities.igdb_client import (
    get_igdb_headers,
)
from utilities.rate_limit import create_steam_session, get_appdetails_limiter

from utilities.igdb_multiquery import (
    BATCH_SIZE,
    MULTIQUERY_URL,
    build_external_games_multiquery,
    build_pop_multiquery,
    flatten_multiquery,
)

logger = logging.getLogger(__name__)

STEAM_CHARTS_URL = (
    "https://api.steampowered.com/ISteamChartsService/GetMostPlayedGames/v1/"
)
STEAM_APPDETAILS_URL = "https://store.steampowered.com/api/appdetails"


@task(retries=0)
def fetch_popularity_data() -> tuple[set[int], list[tuple]]:
    resp = niquests.post(
        MULTIQUERY_URL,
        headers=get_igdb_headers(),
        data=build_pop_multiquery(),
        timeout=60,
    )
    resp.raise_for_status()
    game_ids: set[int] = set()
    rows: list[tuple] = []
    now = datetime.now(timezone.utc)
    for entry in resp.json():
        for record in flatten_multiquery(entry):
            game_ids.add(record["game_id"])
            rows.append(
                (
                    record["game_id"],
                    record["popularity_type"],
                    record["value"],
                    record["calculated_at"],
                    now,
                )
            )

    logger.info("IGDB popularity selection: %d candidate games", len(game_ids))
    return game_ids, rows


@task(retries=0)
def fetch_steam_app_ids(game_ids: set[int]) -> list[tuple[int, int]]:
    if not game_ids:
        return []
    ids = sorted(game_ids)
    batches = [ids[i : i + BATCH_SIZE] for i in range(0, len(ids), BATCH_SIZE)]
    body = build_external_games_multiquery(batches)
    resp = niquests.post(
        MULTIQUERY_URL, headers=get_igdb_headers(), data=body, timeout=60
    )
    resp.raise_for_status()
    results: list[tuple[int, int]] = []
    seen: set[tuple[int, int]] = set()
    for entry in resp.json():
        for record in flatten_multiquery(entry):
            game_id = record.get("game")
            uid = record.get("uid")
            if game_id is not None and uid is not None:
                try:
                    pair = (game_id, int(uid))
                except (ValueError, TypeError):
                    continue
                if pair not in seen:
                    seen.add(pair)
                    results.append(pair)

    resolved_game_ids = {game_id for game_id, _ in results}
    missing_game_ids = game_ids - resolved_game_ids
    if missing_game_ids:
        logger.warning(
            "IGDB external-games lookup returned no Steam mapping for %d of %d candidate games; "
            "checking the catalog database.",
            len(missing_game_ids),
            len(game_ids),
        )
        for pair in fetch_catalog_steam_app_ids(missing_game_ids):
            if pair not in seen:
                seen.add(pair)
                results.append(pair)

    logger.info(
        "Steam tracking selection: %d candidate games, %d resolved games, %d app mappings.",
        len(game_ids),
        len({game_id for game_id, _ in results}),
        len(results),
    )
    return results


@task(retries=0)
def fetch_steam_chart_app_ids() -> list[int]:
    resp = niquests.get(STEAM_CHARTS_URL, timeout=60)
    resp.raise_for_status()
    ranks = (resp.json().get("response") or {}).get("ranks") or []

    app_ids: list[int] = []
    seen: set[int] = set()
    for rank in ranks:
        try:
            app_id = int(rank["appid"])
        except (KeyError, TypeError, ValueError):
            continue
        if app_id > 0 and app_id not in seen:
            seen.add(app_id)
            app_ids.append(app_id)

    logger.info("Steam charts returned %d app candidates.", len(app_ids))
    return app_ids


@task(retries=0)
def fetch_steam_chart_mappings(app_ids: list[int]) -> list[tuple[int, int]]:
    if not app_ids:
        return []

    results: list[tuple[int, int]] = []
    seen: set[tuple[int, int]] = set()
    for start in range(0, len(app_ids), BATCH_SIZE):
        batch = app_ids[start : start + BATCH_SIZE]
        values = ",".join(f'"{app_id}"' for app_id in batch)
        body = (
            "fields game, uid; "
            f"where uid = ({values}) & external_game_source = 1; "
            "limit 500;"
        )
        resp = niquests.post(
            "https://api.igdb.com/v4/external_games",
            headers=get_igdb_headers(),
            data=body,
            timeout=60,
        )
        resp.raise_for_status()
        for record in resp.json():
            try:
                pair = (int(record["game"]), int(record["uid"]))
            except (KeyError, TypeError, ValueError):
                continue
            if pair not in seen:
                seen.add(pair)
                results.append(pair)

    logger.info(
        "Steam chart mapping: %d chart apps, %d IGDB mappings, %d unmapped apps.",
        len(app_ids),
        len(results),
        len(set(app_ids) - {app_id for _, app_id in results}),
    )
    return results


def merge_tracking_rows(*row_sets: list[tuple[int, int]]) -> list[tuple[int, int]]:
    """Keep one IGDB mapping for each Steam AppID."""
    by_app_id: dict[int, tuple[int, int]] = {}
    for rows in row_sets:
        for game_id, steam_app_id in rows:
            existing = by_app_id.get(steam_app_id)
            candidate = (game_id, steam_app_id)
            if existing is None or candidate < existing:
                by_app_id[steam_app_id] = candidate

    result = sorted(by_app_id.values(), key=lambda row: row[1])
    logger.info(
        "Steam tracking snapshot: %d mappings reduced to %d unique AppIDs.",
        sum(len(rows) for rows in row_sets),
        len(result),
    )
    return result


def canonicalize_tracking_rows(rows: list[tuple[int, int]]) -> list[tuple[int, int]]:
    games: dict[int, set[int]] = {}
    for game_id, steam_app_id in rows:
        games.setdefault(game_id, set()).add(steam_app_id)

    ambiguous_apps = sorted(
        steam_app_id
        for app_ids in games.values()
        if len(app_ids) > 1
        for steam_app_id in app_ids
    )
    if not ambiguous_apps:
        return merge_tracking_rows(rows)

    session = create_steam_session(
        limiter=get_appdetails_limiter(), include_retry_hook=False
    )
    canonical_ids: dict[int, int] = {}
    for steam_app_id in ambiguous_apps:
        try:
            response = session.get(
                STEAM_APPDETAILS_URL,
                params={"appids": str(steam_app_id)},
                timeout=(5, 15),
            )
            response.raise_for_status()
            entry = response.json().get(str(steam_app_id), {})
            data = entry.get("data") if entry.get("success") else None
            if isinstance(data, dict) and data.get("steam_appid") is not None:
                canonical_ids[steam_app_id] = int(data["steam_appid"])
        except Exception as exc:
            logger.warning("Could not resolve Steam AppID %s: %s", steam_app_id, exc)

    resolved = [
        (game_id, canonical_ids.get(steam_app_id, steam_app_id))
        for game_id, steam_app_id in rows
    ]
    logger.info(
        "Resolved %d Steam aliases.",
        sum(canonical_ids.get(app_id, app_id) != app_id for _, app_id in rows),
    )
    return merge_tracking_rows(resolved)


def fetch_catalog_steam_app_ids(game_ids: set[int]) -> list[tuple[int, int]]:
    if not game_ids:
        return []

    conn = get_connection()
    try:
        with conn.cursor() as cur:
            cur.execute(
                "SELECT game, uid FROM igdb.external_games "
                "WHERE game = ANY(%s) AND external_game_source = 1",
                (list(game_ids),),
            )
            rows = cur.fetchall()
    finally:
        conn.close()

    results: list[tuple[int, int]] = []
    for game_id, uid in rows:
        try:
            results.append((int(game_id), int(uid)))
        except (TypeError, ValueError):
            logger.warning(
                "Ignoring non-numeric Steam mapping for game %s: %s", game_id, uid
            )
    return results


@task(retries=0)
def write_tracking_snapshot(rows: list[tuple[int, int]], score_rows: list[tuple]):
    if not rows:
        return
    conn = get_connection()
    now = datetime.now(timezone.utc)
    with conn:
        conn.execute("TRUNCATE TABLE steam.tracked_games")
        with conn.cursor() as cur:
            cur.executemany(
                "INSERT INTO steam.tracked_games (game_id, steam_app_id, refreshed_at) "
                "VALUES (%s, %s, %s)",
                [(game_id, steam_app_id, now) for game_id, steam_app_id in rows],
            )
        if score_rows:
            with conn.cursor() as cur:
                cur.executemany(
                    "INSERT INTO steam_raw.popularity_scores "
                    "(game_id, popularity_type, value, calculated_at, fetched_at) "
                    "VALUES (%s, %s, %s, %s, %s)",
                    score_rows,
                )
    conn.close()


@flow
def steam_game_index():
    with concurrency("pipeline-lock", occupy=1):
        game_ids, score_rows = fetch_popularity_data()
        results = fetch_steam_app_ids(game_ids) if game_ids else []
        chart_app_ids = fetch_steam_chart_app_ids()
        chart_rows = fetch_steam_chart_mappings(chart_app_ids)
        results = merge_tracking_rows(results, chart_rows)
        results = canonicalize_tracking_rows(results)
        if not results:
            return
        write_tracking_snapshot(results, score_rows)
        fill_igdb_gaps()


@task(retries=0)
def fill_igdb_gaps():
    conn = get_connection()
    with conn:
        rows = conn.execute("""
            SELECT t.game_id
            FROM steam.tracked_games t
            LEFT JOIN igdb_source.games g ON t.game_id = g.id
            WHERE g.id IS NULL
        """).fetchall()
    conn.close()
    if not rows:
        logger.info("No missing games — IGDB is up to date.")
        return
    logger.info("Missing %d games — triggering incremental IGDB pipeline", len(rows))
    dlt_igdb_default()


if __name__ == "__main__":
    steam_game_index()
