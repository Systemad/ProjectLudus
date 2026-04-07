from typing import Any, cast

import dlt
from dlt.common.schema.typing import TTableSchemaColumns
from dlt.sources.sql_database import sql_table
from dlt_typesense import typesense as typesense_destination
from dlt_typesense import typesense_adapter
from dlt_typesense.typesense_client import TypesenseClient

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
NUMERIC_TYPE_HINTS: dict[str, dict[str, Any]] = {
    "total_visits": {"name": "total_visits", "x-typesense-type": "int64"}
}


_original_make_field_schema = TypesenseClient._make_field_schema


def _make_field_schema_with_explicit_type(
    self: TypesenseClient, column: dict[str, Any]
) -> dict[str, Any]:
    field_schema = _original_make_field_schema(self, column)
    explicit_type = column.get("x-typesense-type")
    if explicit_type:
        field_schema["type"] = explicit_type
    return field_schema


setattr(TypesenseClient, "_make_field_schema", _make_field_schema_with_explicit_type)


@dlt.source(name="postgres_to_typesense")
def _typesense_source():
    games_resource = sql_table(
        credentials=dlt.secrets["destination.postgres.credentials"],
        table="games_search",
        schema="public",
        defer_table_reflect=True,
        write_disposition="replace",
        # chunk_size=50000,
    )

    array_type_hints = cast(
        TTableSchemaColumns,
        {
            column_name: {"name": column_name, "x-typesense-type": "string[]"}
            for column_name in ARRAY_STRING_FIELDS
        },
    )
    column_type_hints = cast(
        TTableSchemaColumns,
        {
            **array_type_hints,
            **NUMERIC_TYPE_HINTS,
        },
    )
    games_resource.apply_hints(columns=column_type_hints)

    games = typesense_adapter(
        games_resource,
        facet=GAMES_FACETS,
        sort=GAMES_SORT,
    )

    return games


# dagster_dlt component loader expects a concrete DltSource object, not a source factory.
typesense_source = _typesense_source()


typesense_source_pipeline = dlt.pipeline(
    pipeline_name="postgres_to_typesense_pipeline",
    destination=typesense_destination(),
    dataset_name="search",
    progress="log",
    dev_mode=False,
)
