import dlt
from dlt.destinations.impl.postgres.configuration import PostgresCredentials
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


def _postgres_credentials():
    prefix = "destination.postgres.credentials"

    return PostgresCredentials(
        {
            "host": dlt.secrets[f"{prefix}.host"],
            "port": dlt.secrets[f"{prefix}.port"],
            "database": dlt.secrets[f"{prefix}.database"],
            "username": dlt.secrets[f"{prefix}.username"],
            "password": dlt.secrets[f"{prefix}.password"],
        }
    )


def _run_index(table, facets, sort, pipeline_name, dataset_name):
    resource = typesense_adapter(
        sql_table(
            credentials=_postgres_credentials(),
            table=table,
            schema="igdb",
            defer_table_reflect=True,
            write_disposition="merge",
            primary_key="id",
        ),
        facet=facets,
        sort=sort,
    )
    dlt.pipeline(
        pipeline_name=pipeline_name,
        destination=typesense_destination(),
        dataset_name=dataset_name,
        progress="log",
        dev_mode=False,
    ).run(resource)


def run_games():
    _run_index(
        table="games_search",
        facets=GAMES_FACETS,
        sort=GAMES_SORT,
        pipeline_name="postgres_to_typesense_pipeline",
        dataset_name="games",
    )


def run_companies():
    _run_index(
        table="company_search",
        facets=COMPANY_FACETS,
        sort=COMPANY_SORT,
        pipeline_name="search_company_search_pipeline",
        dataset_name="companies",
    )
