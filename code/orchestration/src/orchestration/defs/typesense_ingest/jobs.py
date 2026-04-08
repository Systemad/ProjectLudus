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

GAMES_SORT = [
    "aggregated_rating",
    "release_year",
    "igdb_visits",
    "igdb_want_to_play",
    "igdb_playing",
    "igdb_played",
    "steam_24hr_peak_players",
    "steam_positive_reviews",
    "steam_negative_reviews",
    "steam_total_reviews",
    "steam_global_top_sellers",
    "steam_most_wishlisted_upcoming",
    "twitch_24hr_hours_watched",
]
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
    "total_visits": {"name": "total_visits", "x-typesense-type": "int64"},
    "igdb_visits": {"name": "igdb_visits", "x-typesense-type": "float"},
    "igdb_want_to_play": {"name": "igdb_want_to_play", "x-typesense-type": "float"},
    "igdb_playing": {"name": "igdb_playing", "x-typesense-type": "float"},
    "igdb_played": {"name": "igdb_played", "x-typesense-type": "float"},
    "steam_24hr_peak_players": {
        "name": "steam_24hr_peak_players",
        "x-typesense-type": "float",
    },
    "steam_positive_reviews": {
        "name": "steam_positive_reviews",
        "x-typesense-type": "float",
    },
    "steam_negative_reviews": {
        "name": "steam_negative_reviews",
        "x-typesense-type": "float",
    },
    "steam_total_reviews": {
        "name": "steam_total_reviews",
        "x-typesense-type": "float",
    },
    "steam_global_top_sellers": {
        "name": "steam_global_top_sellers",
        "x-typesense-type": "float",
    },
    "steam_most_wishlisted_upcoming": {
        "name": "steam_most_wishlisted_upcoming",
        "x-typesense-type": "float",
    },
    "twitch_24hr_hours_watched": {
        "name": "twitch_24hr_hours_watched",
        "x-typesense-type": "float",
    },
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
