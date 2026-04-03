from typing import Any

import dlt
from dlt.sources.sql_database import sql_table
from dlt_typesense import typesense_adapter

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
    "clicked": {"name": "clicked", "x-typesense-type": "int64"}
}


@dlt.source(name="postgres_to_typesense")
def make_typesense_index():
    games_resource = sql_table(
        table="games_search",
        schema="public",
        # chunk_size=50000,
    )

    games_resource = typesense_adapter(
        games_resource,
        facet=GAMES_FACETS,
        sort=GAMES_SORT,
    )

    array_type_hints: dict[str, dict[str, Any]] = {
        column_name: {"name": column_name, "x-typesense-type": "string[]"}
        for column_name in ARRAY_STRING_FIELDS
    }
    column_type_hints: dict[str, dict[str, Any]] = {
        **array_type_hints,
        **NUMERIC_TYPE_HINTS,
    }
    games_resource.apply_hints(columns=column_type_hints)  # type: ignore[arg-type]

    return games_resource


pipeline = dlt.pipeline(
    pipeline_name="postgres_to_typesense_pipeline",
    destination="typesense",
    dataset_name="games_search",
    progress="alive_progress",
    dev_mode=False,
)
