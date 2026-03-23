import json
import os
from time import monotonic
from typing import Any, cast

import psycopg
import typesense
from dotenv import load_dotenv
from psycopg import sql
from psycopg.rows import dict_row
from typesense.exceptions import ObjectNotFound

load_dotenv()

SOURCE_SCHEMA: str = "public"
TYPESENSE_BATCH_SIZE: int = 500
PROGRESS_LOG_EVERY_BATCHES: int = 5

GAMES_TYPESENSE_SCHEMA: dict[str, Any] = {
    "fields": [
        {"name": "name", "type": "string", "optional": True},
        {"name": "summary", "type": "string", "optional": True},
        {"name": "storyline", "type": "string", "optional": True},
        {"name": "cover_url", "type": "string", "optional": True},
        {"name": "game_type", "type": "string", "facet": True, "optional": True},
        {"name": "game_status", "type": "string", "facet": True, "optional": True},
        {"name": "themes", "type": "string[]", "facet": True, "optional": True},
        {"name": "genres", "type": "string[]", "facet": True, "optional": True},
        {"name": "game_modes", "type": "string[]", "facet": True, "optional": True},
        {"name": "platforms", "type": "string[]", "facet": True, "optional": True},
        {"name": "game_engines", "type": "string[]", "facet": True, "optional": True},
        {
            "name": "player_perspectives",
            "type": "string[]",
            "facet": True,
            "optional": True,
        },
        {"name": "publishers", "type": "string[]", "facet": True, "optional": True},
        {"name": "developers", "type": "string[]", "facet": True, "optional": True},
        {
            "name": "multiplayer_modes",
            "type": "string[]",
            "facet": True,
            "optional": True,
        },
        {"name": "updated_at", "type": "int64", "optional": True},
        {"name": "first_release_date", "type": "int64", "optional": True},
        {"name": "release_year", "type": "int32", "facet": True, "optional": True},
        {"name": "aggregated_rating", "type": "float"},
        {"name": "aggregated_rating_count", "type": "int32", "optional": True},
        {"name": "rating", "type": "float", "optional": True},
        {"name": "rating_count", "type": "int32", "optional": True},
        {"name": "total_rating", "type": "float", "optional": True},
        {"name": "total_rating_count", "type": "int32", "optional": True},
    ],
    "default_sorting_field": "aggregated_rating",
}

COMPANIES_TYPESENSE_SCHEMA: dict[str, Any] = {
    "fields": [
        {"name": "name", "type": "string", "optional": True},
        {"name": "description", "type": "string", "optional": True},
        {"name": "slug", "type": "string", "optional": True},
        {"name": "url", "type": "string", "optional": True},
        {"name": "logo_url", "type": "string", "optional": True},
        {"name": "status", "type": "string", "facet": True, "optional": True},
        {"name": "updated_at", "type": "int64"},
        {"name": "start_date", "type": "int64", "optional": True},
        {"name": "start_year", "type": "int32", "facet": True, "optional": True},
        {"name": "parent_company", "type": "string", "optional": True},
        {"name": "changed_company", "type": "string", "optional": True},
        {"name": "games_developed_count", "type": "int32"},
        {"name": "games_published_count", "type": "int32"},
    ],
    "default_sorting_field": "updated_at",
}

SYNC_TARGETS: list[dict[str, Any]] = [
    {
        "source_table": "games_search",
        "id_column": "id",
        "collection": "games",
        "schema": GAMES_TYPESENSE_SCHEMA,
    },
    {
        "source_table": "company_search",
        "id_column": "id",
        "collection": "companies",
        "schema": COMPANIES_TYPESENSE_SCHEMA,
    },
]


def get_required_env(name: str) -> str:
    value: str | None = os.environ.get(name)
    if not value:
        raise RuntimeError(f"Missing required environment variable: {name}")
    return value


def get_postgres_conninfo() -> str:
    # Aspire WithReference resource-scoped variables.
    host: str | None = os.environ.get("CATALOGDEV_HOST")
    dbname: str | None = os.environ.get("CATALOGDEV_DATABASENAME")
    user: str | None = os.environ.get("CATALOGDEV_USERNAME")
    password: str | None = os.environ.get("CATALOGDEV_PASSWORD")
    port: str = os.environ.get("CATALOGDEV_PORT", "5432")
    if host and dbname and user and password:
        return (
            f"host={host} port={port} dbname={dbname} user={user} password={password}"
        )

    raise RuntimeError(
        "Missing Postgres connection settings. Set all CATALOGDEV_* variables."
    )


def get_typesense_client() -> typesense.Client:
    host: str = get_required_env("TYPESENSE_HOST")
    port: int = int(os.environ.get("TYPESENSE_PORT", "8108"))
    protocol: str = os.environ.get("TYPESENSE_PROTOCOL", "http")
    api_key: str = get_required_env("TYPESENSE_API_KEY")

    return typesense.Client(
        {
            "nodes": [{"host": host, "port": port, "protocol": protocol}],
            "api_key": api_key,
            "connection_timeout_seconds": 30,
        }
    )


def ensure_collection_exists(
    client: typesense.Client,
    collection: str,
    schema_fields: list[dict[str, Any]],
    default_sorting_field: str,
) -> None:
    schema: dict[str, Any] = {
        "name": collection,
        "fields": [{"name": "id", "type": "string"}, *schema_fields],
        "default_sorting_field": default_sorting_field,
    }

    try:
        client.collections[collection].retrieve()
        client.collections[collection].delete()
        print(f"Deleted existing Typesense collection '{collection}'.")
    except ObjectNotFound:
        pass

    client.collections.create(cast(Any, schema))
    print(f"Created Typesense collection '{collection}'.")


def import_batch(
    client: typesense.Client,
    collection: str,
    rows: list[dict[str, Any]],
    id_column: str,
) -> None:
    docs: list[dict[str, Any]] = []
    for row in rows:
        doc: dict[str, Any] = dict(row)
        if id_column not in doc:
            raise RuntimeError(f"Missing id column '{id_column}' in row for Typesense")
        doc[id_column] = str(doc[id_column])
        docs.append(doc)
    jsonl: str = "\n".join(json.dumps(doc) for doc in docs)
    client.collections[collection].documents.import_(jsonl, {"action": "upsert"})


def sync() -> None:
    sync_started_at: float = monotonic()
    conninfo: str = get_postgres_conninfo()
    client: typesense.Client = get_typesense_client()

    print("Starting Typesense full sync...")
    with psycopg.connect(conninfo, autocommit=False) as conn:
        for target in SYNC_TARGETS:
            source_table: str = target["source_table"]
            id_column: str = target["id_column"]
            collection: str = target["collection"]
            schema_config: dict[str, Any] = target["schema"]

            ensure_collection_exists(
                client,
                collection,
                cast(list[dict[str, Any]], schema_config["fields"]),
                cast(str, schema_config["default_sorting_field"]),
            )

            query: sql.Composed = sql.SQL(
                """
                SELECT *
                FROM {source_schema}.{source_table}
                ORDER BY {id_column} ASC
                """
            ).format(
                source_schema=sql.Identifier(SOURCE_SCHEMA),
                source_table=sql.Identifier(source_table),
                id_column=sql.Identifier(id_column),
            )

            total_rows: int = 0
            batch_num: int = 0
            query_started_at: float = monotonic()
            print(f"Running source query for '{source_table}'...")

            cursor_name: str = f"typesense_sync_cursor_{collection}"
            with conn.cursor(name=cursor_name, row_factory=dict_row) as cur:
                cur.itersize = TYPESENSE_BATCH_SIZE
                cur.execute(query)
                print(
                    "Source query submitted in "
                    f"{monotonic() - query_started_at:.2f}s. Waiting for first batch..."
                )

                while True:
                    fetch_started_at: float = monotonic()
                    rows: list[dict[str, Any]] = cur.fetchmany(TYPESENSE_BATCH_SIZE)
                    if not rows:
                        break

                    if batch_num == 0:
                        print(
                            "First batch fetched "
                            f"({len(rows)} rows) in {monotonic() - query_started_at:.2f}s."
                        )

                    import_started_at: float = monotonic()
                    import_batch(client, collection, rows, id_column)
                    total_rows += len(rows)
                    batch_num += 1

                    if batch_num == 1:
                        print(
                            "First batch imported in "
                            f"{monotonic() - import_started_at:.2f}s "
                            f"(fetch took {import_started_at - fetch_started_at:.2f}s)."
                        )

                    if batch_num % PROGRESS_LOG_EVERY_BATCHES == 0:
                        elapsed: float = monotonic() - sync_started_at
                        print(
                            f"Reindex progress ({collection}): "
                            f"{total_rows} rows imported across {batch_num} batches "
                            f"in {elapsed:.2f}s."
                        )

            print(
                f"Typesense sync complete for '{collection}'. "
                f"Upserted {total_rows} rows."
            )

    print(
        f"All Typesense sync targets complete in {monotonic() - sync_started_at:.2f}s."
    )


if __name__ == "__main__":
    sync()
