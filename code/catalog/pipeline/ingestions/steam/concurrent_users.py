import logging
import os
from datetime import datetime, timezone

from alive_progress import alive_bar
from utilities._tracked_games import get_tracked_games
from utilities.database import get_connection
from utilities.rate_limit import STEAM_SAFE_LIMITER, create_steam_session

STEAM__API_KEY = os.getenv("STEAM__API_KEY")

logger = logging.getLogger(__name__)


def steam_ccu():
    if not STEAM__API_KEY:
        raise ValueError("Missing required environment variable: STEAM__API_KEY")

    conn = None
    try:
        conn = get_connection()
        rows = get_tracked_games(conn)

        session = create_steam_session(
            limiter=STEAM_SAFE_LIMITER, include_retry_hook=True
        )
        sql_rows = []
        with alive_bar(len(rows), title="concurrent_users") as bar:
            for game_id, steam_app_id in rows:
                try:
                    resp = session.get(
                        "https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/",
                        params={"appid": str(steam_app_id), "key": STEAM__API_KEY},
                    )
                    resp.raise_for_status()
                    body = resp.json()
                    current_players = body["response"]["player_count"]
                except Exception:
                    current_players = 0

                sql_rows.append(
                    {
                        "game_id": game_id,
                        "steam_app_id": steam_app_id,
                        "current_players": current_players,
                        "captured_at": datetime.now(timezone.utc),
                    }
                )
                bar()

        with conn.cursor() as cur:
            for row in sql_rows:
                cur.execute(
                    "INSERT INTO steam_raw.concurrent_users (game_id, steam_app_id, current_players, captured_at) "
                    "VALUES (%s, %s, %s, %s) ON CONFLICT DO NOTHING",
                    (
                        row["game_id"],
                        row["steam_app_id"],
                        row["current_players"],
                        row["captured_at"],
                    ),
                )
        conn.commit()
    finally:
        if conn:
            conn.close()


if __name__ == "__main__":
    steam_ccu()
