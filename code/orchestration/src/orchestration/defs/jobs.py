from dagster import AssetSelection, define_asset_job

igdb_default_dlt_job = define_asset_job(
    name="igdb_default_dlt_job",
    selection=AssetSelection.groups("igdb_default"),
    tags={"type": "dlt"},
)

igdb_popularity_dlt_job = define_asset_job(
    name="igdb_popularity_dlt_job",
    selection=AssetSelection.groups("igdb_popularity_data"),
    tags={"type": "dlt"},
)

dbt_transformation_job = define_asset_job(
    name="dbt_transformation_job",
    selection=AssetSelection.groups("igdb_dbt_transformations"),
)

make_typesense_index_job = define_asset_job(
    name="make_typesense_index_job",
    selection=AssetSelection.groups("typesense_job"),
    tags={"type": "dlt", "target": "typesense"},
)

gameclickcount_copy_job = define_asset_job(
    name="gameclickcount_copy_job",
    selection=AssetSelection.groups("gameclickcount_copy"),
    tags={"type": "dlt", "target": "postgres"},
)

full_pipeline_job = define_asset_job(
    name="full_pipeline_job",
    selection=AssetSelection.groups(
        "igdb_popularity_data",
        "igdb_default",
        "igdb_dbt_transformations",
        "typesense_job",
        "gameclickcount_copy",
    ),
)
