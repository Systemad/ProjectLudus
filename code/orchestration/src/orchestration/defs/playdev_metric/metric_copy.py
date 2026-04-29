import dlt
from dlt.sources.sql_database import sql_table


@dlt.source(name="metric_copy")
def _metric_copy_source():
    resource = sql_table(
        credentials=dlt.secrets["sources.playdev_postgres.credentials"],
        table="game_metrics",
        schema="public",
        defer_table_reflect=False,
    ).with_name("game_metrics")

    resource = resource.apply_hints(
        primary_key="id",
        columns={  # type: ignore[arg-type]
            "id": {"data_type": "bigint"},
            "session_id": {"data_type": "text"},
            "game_id": {"data_type": "bigint"},
            "first_visited_at": {"data_type": "timestamp", "timezone": True},
            "last_visited_at": {"data_type": "timestamp", "timezone": True},
            "view_count": {"data_type": "bigint"},
        },
    )

    return resource


# dagster_dlt component loader expects a concrete DltSource object, not a source factory.
metric_copy_source = _metric_copy_source()


metric_copy_pipeline = dlt.pipeline(
    pipeline_name="metric_copy_pipeline",
    destination="postgres",
    dataset_name="igdb_source",
    progress="log",
    dev_mode=False,
)
