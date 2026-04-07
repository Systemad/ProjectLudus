import dlt
from dlt.sources.sql_database import sql_table


@dlt.source(name="gameclickcount_copy")
def _gameclickcount_copy_source():
    resource = sql_table(
        credentials=dlt.secrets["sources.gameclickcount_postgres.credentials"],
        table="game_visit_counts",
        schema="public",
        defer_table_reflect=True,
    ).with_name("game_visit_counts")

    return resource


# dagster_dlt component loader expects a concrete DltSource object, not a source factory.
gameclickcount_copy_source = _gameclickcount_copy_source()


gameclickcount_copy_pipeline = dlt.pipeline(
    pipeline_name="gameclickcount_copy_pipeline",
    destination="postgres",
    dataset_name="igdb_source",
    progress="log",
    dev_mode=False,
)
