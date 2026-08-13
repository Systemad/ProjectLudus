import logging
from datetime import datetime, timezone

from alive_progress import alive_bar
from utilities._tracked_games import get_tracked_games
from utilities.database import get_connection
from utilities.rate_limit import get_appdetails_limiter, create_steam_session
from utilities.steam_urls import STEAM_APPDETAILS_URL

logger = logging.getLogger(__name__)


def run():
    conn = None
    try:
        logger.info("connecting to database...")
        conn = get_connection()
        logger.info("fetching tracked games...")
        rows = get_tracked_games(conn)
        logger.info("got %d tracked games", len(rows))

        now = datetime.now(timezone.utc)
        session = create_steam_session(
            limiter=get_appdetails_limiter(), include_retry_hook=False
        )
        details_rows = []

        with alive_bar(len(rows), title="Fetching Steam game details") as bar:
            for game_id, steam_app_id in rows:
                try:
                    resp = session.get(
                        STEAM_APPDETAILS_URL,
                        params={
                            "appids": str(steam_app_id),
                            "cc": "SE",
                            "filters": "basic,price_overview",
                            "l": "english",
                        },
                        timeout=(5, 15),
                    )
                    resp.raise_for_status()
                    body = resp.json()
                    entry = body.get(str(steam_app_id)) or {}
                    if not entry.get("success") or "data" not in entry:
                        continue

                    data = entry["data"]

                    details_rows.append(
                        {
                            "game_id": game_id,
                            "steam_app_id": steam_app_id,
                            "header_url": data.get("header_image", ""),
                            "capsule_url": data.get("capsule_image", ""),
                            "captured_at": now,
                        }
                    )
                except Exception as e:
                    logger.error("error for app %s: %s", steam_app_id, e)
                finally:
                    bar()

        if not details_rows:
            logger.warning("No store details to insert.")
            return

        logger.info("API requests done, inserting %d rows...", len(details_rows))
        with conn.cursor() as cur:
            cur.executemany(
                "INSERT INTO steam_raw.store_details "
                "(game_id, steam_app_id, header_url, capsule_url, captured_at) "
                "VALUES (%(game_id)s, %(steam_app_id)s, %(header_url)s, %(capsule_url)s, %(captured_at)s) "
                "ON CONFLICT (game_id) DO UPDATE SET"
                " header_url = EXCLUDED.header_url,"
                " capsule_url = EXCLUDED.capsule_url,"
                " captured_at = EXCLUDED.captured_at",
                details_rows,
            )
        conn.commit()
        logger.info("Inserted %d detail rows.", len(details_rows))
    finally:
        if conn:
            conn.close()


if __name__ == "__main__":
    run()
