import os
from typing import Any, Iterator, Literal
from urllib.parse import quote_plus

import connectorx as cx
import dlt
from dlt.sources.credentials import ConnectionStringCredentials
from dlt_typesense import typesense_adapter
from dlt_typesense.typesense_client import TypesenseClient

CHUNKSIZE = int(os.getenv("CHUNKSIZE", 50000))


if not hasattr(TypesenseClient, "_pl_typesense_type_patch"):
    _orig_make_field_schema = TypesenseClient._make_field_schema

    def _patched_make_field_schema(self, column):
        field_schema = _orig_make_field_schema(self, column)
        forced_type = column.get("x-typesense-type")
        if forced_type:
            field_schema["type"] = forced_type
        return field_schema

    setattr(TypesenseClient, "_make_field_schema", _patched_make_field_schema)
    setattr(TypesenseClient, "_pl_typesense_type_patch", True)


def read_sql_x_chunked(
    conn_str: str, query: str, chunk_size: int = CHUNKSIZE
) -> Iterator:
    offset = 0
    while True:
        chunk_query = f"{query} LIMIT {chunk_size} OFFSET {offset}"
        data_chunk = cx.read_sql(
            conn_str,
            chunk_query,
            return_type="arrow",
            protocol="binary",
        )
        yield data_chunk
        if data_chunk.num_rows < chunk_size:
            break
        offset += chunk_size


@dlt.source(max_table_nesting=0)
def pg_games_source(
    table_name: str = "games_search",
    primary_key: list[str] | None = None,
    schema_name: str = "public",
    order_date: str = "id",
    load_type: Literal["append", "replace", "merge", "skip"] = "replace",
    columns: str = "*",
    credentials: ConnectionStringCredentials | str | None = None,
):
    """Native dlt Postgres source using connectorx chunked reads.

    Pass `credentials` (ConnectionStringCredentials) or rely on `dlt.secrets["sources.postgres.credentials"]`.
    """
    if primary_key is None:
        primary_key = ["id"]

    # resolve connection string
    if credentials is None:
        # accept either source-style or destination-style secret paths
        for secret_path in (
            "sources.postgres.credentials",
            "destination.postgres.credentials",
        ):
            try:
                credentials = dlt.secrets[secret_path]
                if credentials is not None:
                    break
            except Exception:
                credentials = None

    if credentials is None:
        raise RuntimeError(
            "Postgres credentials required: provide `credentials` or set dlt.secrets['sources.postgres.credentials']"
        )

    if isinstance(credentials, ConnectionStringCredentials):
        conn_str = credentials.to_native_representation()
    elif isinstance(credentials, str):
        conn_str = credentials
    elif isinstance(credentials, dict):
        username = credentials.get("username", "")
        password = quote_plus(str(credentials.get("password", "")))
        host = credentials.get("host", "localhost")
        port = credentials.get("port", 5432)
        database = credentials.get("database", "postgres")
        conn_str = f"postgresql://{username}:{password}@{host}:{port}/{database}"
    else:
        # dlt may return a typed object with postgres fields; build DSN if available
        if all(
            hasattr(credentials, k)
            for k in ("username", "password", "host", "port", "database")
        ):
            username = getattr(credentials, "username")
            password = quote_plus(str(getattr(credentials, "password")))
            host = getattr(credentials, "host")
            port = getattr(credentials, "port")
            database = getattr(credentials, "database")
            conn_str = f"postgresql://{username}:{password}@{host}:{port}/{database}"
        else:
            conn_str = str(credentials)

    query = f"SELECT {columns} FROM {schema_name}.{table_name} ORDER BY {order_date}"

    resource = dlt.resource(
        name=table_name,
        table_name=table_name,
        write_disposition=load_type,
        primary_key=primary_key,
        parallelized=True,
    )(read_sql_x_chunked)(
        conn_str,
        query,
    )

    if load_type == "merge":
        try:
            resource.apply_hints(incremental=dlt.sources.incremental(order_date))
        except Exception:
            # if incremental helper not available, silently continue; dlt will still run
            pass

    resource = typesense_adapter(
        resource,
        facet=GAMES_FACETS,
        sort=GAMES_SORT,
    )

    array_type_hints: dict[str, dict[str, Any]] = {
        c: {"name": c, "x-typesense-type": "string[]"} for c in ARRAY_STRING_FIELDS
    }
    resource.apply_hints(columns=array_type_hints)  # type: ignore[arg-type]

    return resource


# Typesense hints matching schema provided earlier
GAMES_FACETS = [
    "game_type",
    "game_status",
    "themes",
    "genres",
    "game_modes",
    "platforms",
    "game_engines",
    "player_perspectives",
    "multiplayer_modes",
]

GAMES_SORT = ["aggregated_rating", "release_year"]
ARRAY_STRING_FIELDS = [
    "themes",
    "genres",
    "game_modes",
    "platforms",
    "game_engines",
    "player_perspectives",
    "publishers",
    "developers",
    "multiplayer_modes",
]


# Exposed dlt source containing a single Typesense-wrapped resource
make_typesense_index = pg_games_source(
    table_name="games_search",
    primary_key=["id"],
    schema_name="public",
    order_date="id",
    load_type="replace",
    columns="*",
)

# Backward-compatible alias
games = make_typesense_index
