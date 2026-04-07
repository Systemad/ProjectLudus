import os
import sys
from typing import Any, cast

import typesense

RULES: list[dict[str, Any]] = [
    {
        "name": "games_popular_queries",
        "type": "popular_queries",
        "params": {
            "source": {
                "collections": ["search___games_search"],
            },
            "destination": {
                "collection": "analytics",
            },
            "limit": 100,
        },
    },
    {
        "name": "games_clicks_counter",
        "type": "counter",
        "collection": "search___games_search",
        "event_type": "click",
        "params": {
            "destination_collection": "search___games_search",
            "counter_field": "total_visits",
            "weight": 1,
        },
    },
]


def build_client() -> typesense.Client:
    return typesense.Client(
        {
            "api_key": os.environ["TYPESENSE_API_KEY"],
            "nodes": [
                {
                    "host": os.environ["TYPESENSE_HOST"],
                    "port": int(os.environ.get("TYPESENSE_PORT", "8108")),
                    "protocol": os.environ.get("TYPESENSE_PROTOCOL", "http"),
                }
            ],
            "connection_timeout_seconds": 5,
        }
    )


def recreate_rules(client: typesense.Client) -> None:
    for rule in RULES:
        name = cast(str, rule["name"])
        try:
            client.analytics.rules[name].delete()
            print(f"[deleted] {name}")
        except Exception:
            pass
        try:
            client.analytics.rules.create(cast(Any, rule))
            print(f"[created] {name}")
        except Exception as err:
            print(f"[error] failed creating {name}: {err}", file=sys.stderr)
            sys.exit(1)


def main() -> None:
    for key in ("TYPESENSE_HOST", "TYPESENSE_API_KEY"):
        if not os.environ.get(key):
            print(f"[error] missing env var: {key}", file=sys.stderr)
            sys.exit(1)

    recreate_rules(build_client())


if __name__ == "__main__":
    main()
