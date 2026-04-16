import dlt
from dlt.sources.sql_database import sql_table
from dlt_typesense import typesense as typesense_destination
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
    "publishers",
    "developers",
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

COMPANY_FACETS = [
    "status",
]

COMPANY_SORT = [
    "games_developed_count",
    "games_published_count",
    "start_year",
]


@dlt.source(name="postgres_to_typesense")
def _typesense_source():
    games_resource = sql_table(
        credentials=dlt.secrets["destination.postgres.credentials"],
        table="games_search",
        schema="public",
        defer_table_reflect=True,
        write_disposition="merge",
        primary_key="id",
        # chunk_size=50000,
    )

    games = typesense_adapter(
        games_resource,
        facet=GAMES_FACETS,
        sort=GAMES_SORT,
    )

    return games


@dlt.source(name="postgres_to_typesense_companies")
def _company_typesense_source():
    companies_resource = sql_table(
        credentials=dlt.secrets["destination.postgres.credentials"],
        table="company_search",
        schema="public",
        defer_table_reflect=True,
        write_disposition="merge",
        primary_key="id",
        # chunk_size=50000,
    )

    companies = typesense_adapter(
        companies_resource,
        facet=COMPANY_FACETS,
        sort=COMPANY_SORT,
    )

    return companies


# dagster_dlt component loader expects a concrete DltSource object, not a source factory.
typesense_source = _typesense_source()
company_typesense_source = _company_typesense_source()


typesense_source_pipeline = dlt.pipeline(
    pipeline_name="postgres_to_typesense_pipeline",
    destination=typesense_destination(),
    dataset_name="games",
    progress="log",
    dev_mode=False,
)

company_typesense_source_pipeline = dlt.pipeline(
    pipeline_name="search_company_search_pipeline",
    destination=typesense_destination(),
    dataset_name="companies",
    progress="log",
    dev_mode=False,
)
