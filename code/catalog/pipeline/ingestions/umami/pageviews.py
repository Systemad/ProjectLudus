import os
import re
from datetime import date, datetime, timedelta, timezone

from utilities.database import get_connection
import dlt
import niquests

BASE_URL = os.getenv("UMAMI__BASE_URL")
WEBSITE_ID = os.getenv("UMAMI__WEBSITE_ID")
API_KEY = os.getenv("UMAMI__API_KEY")

if not BASE_URL or not WEBSITE_ID or not API_KEY:
    raise ValueError(
        "Missing required environment variables: UMAMI__BASE_URL, UMAMI__WEBSITE_ID, and/or UMAMI__API_KEY"
    )


def fetch_today_pageviews() -> list[dict]:
    today = date.today()
    start = datetime(today.year, today.month, today.day, tzinfo=timezone.utc)
    end = start + timedelta(days=1)

    payload = {
        "startAt": str(int(start.timestamp() * 1000)),
        "endAt": str(int(end.timestamp() * 1000)),
        "type": "path",
        "limit": "250",
    }
    resp = niquests.get(
        f"{BASE_URL}/v1/websites/{WEBSITE_ID}/metrics",
        headers={"Authorization": f"Bearer {API_KEY}"},
        params=payload,
    )
    resp.raise_for_status()

    rows = []
    for item in resp.json():
        m = re.match(r"^/games/(\d+)$", item.get("x", ""))
        if m:
            rows.append(
                {
                    "game_id": int(m.group(1)),
                    "pageviews": item["y"],
                    "date": today,
                }
            )
    return rows


def insert_pageviews(rows: list[dict]):
    conn = get_connection()
    with conn.cursor() as cur:
        for row in rows:
            cur.execute(
                "INSERT INTO umami_raw.pageviews (game_id, pageviews, date) "
                "VALUES (%s, %s, %s) ON CONFLICT DO NOTHING",
                (row["game_id"], row["pageviews"], row["date"]),
            )
    conn.commit()
    conn.close()


def run():
    rows = fetch_today_pageviews()
    if rows:
        insert_pageviews(rows)
