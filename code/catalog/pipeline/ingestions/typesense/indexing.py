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
    "aggregated_rating_count",
    "rating",
    "rating_count",
    "total_rating",
    "total_rating_count",
    "hypes",
    "updated_at",
    "first_release_date_epoch",
    "release_year",
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
        credentials=dlt.secrets.get("destination.postgres.credentials"),
        table="games_search",
        schema="igdb",
        defer_table_reflect=True,
        write_disposition="merge",
        primary_key="id",
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
        credentials=dlt.secrets.get("destination.postgres.credentials"),
        table="company_search",
        schema="igdb",
        defer_table_reflect=True,
        write_disposition="merge",
        primary_key="id",
    )

    companies = typesense_adapter(
        companies_resource,
        facet=COMPANY_FACETS,
        sort=COMPANY_SORT,
    )

    return companies


def run_games():
    ts = typesense_adapter(
        sql_table(
            credentials=dlt.secrets.get("destination.postgres.credentials"),
            table="games_search",
            schema="igdb",
            defer_table_reflect=True,
            write_disposition="merge",
            primary_key="id",
        ),
        facet=GAMES_FACETS,
        sort=GAMES_SORT,
    )
    dlt.pipeline(
        pipeline_name="postgres_to_typesense_pipeline",
        destination=typesense_destination(),
        dataset_name="games",
        progress="log",
        dev_mode=False,
    ).run(ts)


def run_companies():
    cs = typesense_adapter(
        sql_table(
            credentials=dlt.secrets.get("destination.postgres.credentials"),
            table="company_search",
            schema="igdb",
            defer_table_reflect=True,
            write_disposition="merge",
            primary_key="id",
        ),
        facet=COMPANY_FACETS,
        sort=COMPANY_SORT,
    )
    dlt.pipeline(
        pipeline_name="search_company_search_pipeline",
        destination=typesense_destination(),
        dataset_name="companies",
        progress="log",
        dev_mode=False,
    ).run(cs)
