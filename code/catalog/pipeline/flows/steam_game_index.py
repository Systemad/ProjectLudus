from datetime import datetime, timezone

import niquests
from prefect import flow, task
from utilities.database import get_connection
from utilities.igdb_client import (
    get_igdb_headers,
)
from utilities.igdb_multiquery import (
    BATCH_SIZE,
    MULTIQUERY_URL,
    build_external_games_multiquery,
    build_pop_multiquery,
    flatten_multiquery,
)


@task(retries=0)
def fetch_popularity_primitives() -> set[int]:
    resp = niquests.post(
        MULTIQUERY_URL,
        headers=get_igdb_headers(),
        data=build_pop_multiquery(),
        timeout=60,
    )
    resp.raise_for_status()
    game_ids: set[int] = set()
    for entry in resp.json():
        for record in flatten_multiquery(entry):
            game_ids.add(record["game_id"])
    return game_ids


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
    return results


@task(retries=0)
def write_tracked_games(rows: list[tuple[int, int]]):
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
    conn.close()


@flow
def steam_game_index():
    game_ids = fetch_popularity_primitives()
    if not game_ids:
        return
    results = fetch_steam_app_ids(game_ids)
    if not results:
        return
    write_tracked_games(results)


if __name__ == "__main__":
    steam_game_index()
