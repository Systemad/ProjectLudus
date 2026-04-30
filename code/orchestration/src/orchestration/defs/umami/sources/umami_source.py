from datetime import datetime, timedelta, timezone

from dlt.sources.rest_api import RESTAPIConfig


def _epoch_ms(dt: datetime) -> int:
    return int(dt.timestamp() * 1000)


def _previous_calendar_week() -> tuple[datetime, datetime]:
    today = datetime.now(timezone.utc)
    monday = today - timedelta(days=today.weekday())
    start = monday - timedelta(days=7)
    end = monday
    return start, end


def _last_n_days(n: int) -> tuple[datetime, datetime]:
    end = datetime.now(timezone.utc)
    start = end - timedelta(days=n)
    return start, end


def umami_source(
    base_url: str,
    website_id: str,
) -> RESTAPIConfig:
    start, end = _last_n_days(1)

    return {
        "client": {
            "base_url": base_url,
        },
        "resource_defaults": {
            "primary_key": None,
            "write_disposition": "append",
            "endpoint": {
                "params": {
                    "startAt": _epoch_ms(start),
                    "endAt": _epoch_ms(end),
                    "limit": 250,
                },
            },
        },
        "resources": [
            {
                "name": "pageviews_by_path",
                "endpoint": {
                    "path": f"/v1/websites/{website_id}/metrics",
                    "params": {"type": "path"},
                },
            },
        ],
    }
