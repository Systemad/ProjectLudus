import logging
import math
from datetime import datetime, timezone

from alive_progress import alive_bar
from utilities._tracked_games import get_tracked_games
from utilities.database import get_connection
from utilities.rate_limit import create_steam_session
from utilities.steam_urls import STEAM_APPDETAILS_URL

logger = logging.getLogger(__name__)

BATCH_SIZE = 100


def run():
    conn = None
    try:
        conn = get_connection()
        rows = get_tracked_games(conn)

        now = datetime.now(timezone.utc)
        session = create_steam_session(include_retry_hook=False)
        pricing_rows = []
        num_batches = math.ceil(len(rows) / BATCH_SIZE)

        with alive_bar(num_batches, title="store_pricing") as bar:
            for i in range(0, len(rows), BATCH_SIZE):
                batch = rows[i : i + BATCH_SIZE]
                appids = ",".join(str(sid) for _, sid in batch)
                id_map = {sid: gid for gid, sid in batch}

                try:
                    resp = session.get(
                        STEAM_APPDETAILS_URL,
                        params={
                            "appids": appids,
                            "cc": "DE",
                            "filters": "price_overview",
                            "l": "english",
                        },
                    )
                    resp.raise_for_status()
                    body = resp.json()

                    for sid_str, entry in body.items():
                        steam_app_id = int(sid_str)
                        game_id = id_map.get(steam_app_id)
                        if game_id is None:
                            continue

                        if not entry.get("success"):
                            continue

                        data = entry.get("data")

                        if isinstance(data, list) and len(data) == 0:
                            pricing_rows.append(
                                {
                                    "game_id": game_id,
                                    "steam_app_id": steam_app_id,
                                    "initial_cents": 0,
                                    "final_cents": 0,
                                    "discount_percent": 0,
                                    "currency": "EUR",
                                    "initial_formatted": "Free",
                                    "final_formatted": "Free",
                                    "captured_at": now,
                                }
                            )
                        elif isinstance(data, dict):
                            price = data.get("price_overview")
                            if price is not None:
                                pricing_rows.append(
                                    {
                                        "game_id": game_id,
                                        "steam_app_id": steam_app_id,
                                        "initial_cents": price.get("initial", 0),
                                        "final_cents": price.get("final", 0),
                                        "discount_percent": price.get(
                                            "discount_percent", 0
                                        ),
                                        "currency": price.get("currency", ""),
                                        "initial_formatted": price.get(
                                            "initial_formatted", ""
                                        ),
                                        "final_formatted": price.get(
                                            "final_formatted", ""
                                        ),
                                        "captured_at": now,
                                    }
                                )
                except Exception:
                    pass

                bar()

        if not pricing_rows:
            logger.warning("No pricing data to insert.")
            return

        with conn.cursor() as cur:
            cur.executemany(
                "INSERT INTO steam_raw.store_pricing "
                "(game_id, steam_app_id, initial_cents, final_cents, discount_percent, "
                "currency, initial_formatted, final_formatted, captured_at) "
                "VALUES (%(game_id)s, %(steam_app_id)s, %(initial_cents)s, %(final_cents)s, "
                "%(discount_percent)s, %(currency)s, %(initial_formatted)s, %(final_formatted)s, "
                "%(captured_at)s) "
                "ON CONFLICT DO NOTHING",
                pricing_rows,
            )
        conn.commit()
        logger.info("Inserted %d pricing rows.", len(pricing_rows))
    finally:
        if conn:
            conn.close()


if __name__ == "__main__":
    run()
