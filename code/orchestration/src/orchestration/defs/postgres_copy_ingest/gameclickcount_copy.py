import dlt
from dlt.sources.sql_database import sql_table


@dlt.source(name="gameclickcount_copy")
def gameclickcount_copy_source():
    resource = sql_table(
        credentials=dlt.secrets["sources.gameclickcount_postgres.credentials"],
        table="game_visit_counts",
        schema="public",
    ).with_name("gamesclickcount")

    return resource


gameclickcount_copy_pipeline = dlt.pipeline(
    pipeline_name="gameclickcount_copy_pipeline",
    destination="postgres",
    dataset_name="igdb_source",
    progress="alive_progress",
    dev_mode=False,
)
