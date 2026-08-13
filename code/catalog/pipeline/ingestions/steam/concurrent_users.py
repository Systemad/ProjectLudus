import logging
import os
from datetime import datetime, timezone

from alive_progress import alive_bar
from utilities._tracked_games import get_tracked_games
from utilities.database import get_connection
from utilities.rate_limit import get_steam_limiter, create_steam_session

logger = logging.getLogger(__name__)


def steam_ccu():
    steam_api_key = os.environ.get("STEAM__API_KEY")
    if steam_api_key is None:
        raise ValueError("Missing required environment variable: STEAM__API_KEY")

    conn = None
    try:
        conn = get_connection()
        rows = get_tracked_games(conn)

        session = create_steam_session(
            limiter=get_steam_limiter(), include_retry_hook=True
        )
        sql_rows = []
        failed_rows = 0
        with alive_bar(len(rows), title="concurrent_users") as bar:
            for game_id, steam_app_id in rows:
                try:
                    resp = session.get(
                        "https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/",
                        params={"appid": str(steam_app_id), "key": steam_api_key},
                        timeout=(5, 15),
                    )
                    resp.raise_for_status()
                    body = resp.json()
                    current_players = body["response"]["player_count"]
                except Exception as exc:
                    failed_rows += 1
                    logger.warning(
                        "Failed to fetch current players for game %s / Steam app %s: %s",
                        game_id,
                        steam_app_id,
                        exc,
                    )
                else:
                    sql_rows.append(
                        {
                            "game_id": game_id,
                            "steam_app_id": steam_app_id,
                            "current_players": current_players,
                            "captured_at": datetime.now(timezone.utc),
                        }
                    )
                finally:
                    bar()

        if sql_rows:
            with conn.cursor() as cur:
                cur.executemany(
                    "INSERT INTO steam_raw.concurrent_users (game_id, steam_app_id, current_players, captured_at) "
                    "VALUES (%s, %s, %s, %s) ON CONFLICT DO NOTHING",
                    [
                        (
                            row["game_id"],
                            row["steam_app_id"],
                            row["current_players"],
                            row["captured_at"],
                        )
                        for row in sql_rows
                    ],
                )
            conn.commit()

        logger.info(
            "Steam CCU ingestion complete: %d succeeded, %d failed.",
            len(sql_rows),
            failed_rows,
        )
    finally:
        if conn:
            conn.close()


if __name__ == "__main__":
    steam_ccu()
