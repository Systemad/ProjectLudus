import logging
from datetime import datetime, timezone

import niquests
from alive_progress import alive_bar
from utilities._tracked_games import get_tracked_games
from utilities.database import get_connection
from utilities.rate_limit import get_steam_limiter, create_steam_session

logger = logging.getLogger(__name__)


def run():
    conn = None
    try:
        conn = get_connection()
        rows = get_tracked_games(conn)

        now = datetime.now(timezone.utc)
        session = create_steam_session(limiter=get_steam_limiter(), include_retry_hook=False)
        sql_rows = []

        with alive_bar(len(rows), title="steam_reviews") as bar:
            for game_id, steam_app_id in rows:
                try:
                    resp = session.get(
                        f"https://store.steampowered.com/appreviews/{steam_app_id}",
                        params={
                            "json": "1",
                            "language": "all",
                            "filter": "all",
                            "review_type": "all",
                            "purchase_type": "all",
                        },
                    )
                    resp.raise_for_status()
                    body = resp.json()

                    if body.get("success") == 1:
                        summary = body.get("query_summary", {})
                        sql_rows.append(
                            {
                                "game_id": game_id,
                                "steam_app_id": steam_app_id,
                                "num_reviews": summary.get("num_reviews", 0),
                                "review_score": summary.get("review_score", 0),
                                "review_score_desc": summary.get(
                                    "review_score_desc", ""
                                ),
                                "total_positive": summary.get("total_positive", 0),
                                "total_negative": summary.get("total_negative", 0),
                                "total_reviews": summary.get("total_reviews", 0),
                                "captured_at": now,
                            }
                        )
                except Exception:
                    pass

                bar()

        if not sql_rows:
            logger.warning("No reviews data to insert.")
            return

        with conn.cursor() as cur:
            cur.executemany(
                "INSERT INTO steam_raw.reviews "
                "(game_id, steam_app_id, num_reviews, review_score, review_score_desc, "
                "total_positive, total_negative, total_reviews, captured_at) "
                "VALUES (%(game_id)s, %(steam_app_id)s, %(num_reviews)s, %(review_score)s, "
                "%(review_score_desc)s, %(total_positive)s, %(total_negative)s, "
                "%(total_reviews)s, %(captured_at)s) "
                "ON CONFLICT DO NOTHING",
                sql_rows,
            )
        conn.commit()
        logger.info("Inserted %d review rows.", len(sql_rows))
    finally:
        if conn:
            conn.close()


if __name__ == "__main__":
    run()
