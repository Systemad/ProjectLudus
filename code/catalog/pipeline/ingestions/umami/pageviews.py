import os
import re
from datetime import date, datetime, timedelta, timezone

from utilities.database import get_connection
import dlt
import niquests


def fetch_today_pageviews(
    base_url: str, website_id: str, api_key: str
) -> list[dict]:
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
        f"{base_url}/v1/websites/{website_id}/metrics",
        headers={"Authorization": f"Bearer {api_key}"},
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
    base_url = os.environ.get("UMAMI__BASE_URL")
    website_id = os.environ.get("UMAMI__WEBSITE_ID")
    api_key = os.environ.get("UMAMI__API_KEY")
    if not base_url or not website_id or not api_key:
        raise ValueError(
            "Missing required environment variables: UMAMI__BASE_URL, "
            "UMAMI__WEBSITE_ID, and/or UMAMI__API_KEY"
        )

    rows = fetch_today_pageviews(base_url, website_id, api_key)
    if rows:
        insert_pageviews(rows)
