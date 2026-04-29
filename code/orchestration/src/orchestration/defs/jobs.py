from dagster import AssetSelection, define_asset_job, in_process_executor

igdb_dlt_job = define_asset_job(
    name="igdb_dlt_job",
    selection=AssetSelection.groups("igdb_default", "igdb_popularity_data"),
    tags={"type": "dlt"},
    executor_def=in_process_executor,
)

dbt_transformation_job = define_asset_job(
    name="dbt_transformation_job",
    selection=AssetSelection.groups("igdb_dbt_transformations"),
    executor_def=in_process_executor,
)

make_typesense_index_job = define_asset_job(
    name="make_typesense_index_job",
    selection=AssetSelection.groups("typesense_job"),
    tags={"type": "dlt", "target": "typesense"},
    executor_def=in_process_executor,
)

metric_copy_job = define_asset_job(
    name="metric_copy_job",
    selection=AssetSelection.groups("metric_copy"),
    tags={"type": "dlt", "target": "postgres"},
    executor_def=in_process_executor,
)

full_pipeline_job = define_asset_job(
    name="full_pipeline_job",
    selection=AssetSelection.groups(
        "igdb_popularity_data",
        "igdb_default",
        "igdb_dbt_transformations",
        "typesense_job",
        "metric_copy",
    ),
    executor_def=in_process_executor,
)
