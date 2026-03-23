import dagster as dg

# Job for dlt assets (IGDB ingestion)
igdb_ingestion_job = dg.define_asset_job(
    name="igdb_ingestion_job",
    selection=dg.AssetSelection.groups(
        "igdb_rest_ingest"
    ),  # Selects assets from this component
)

# Separate DLT job: default pipeline (non-popularity assets)
igdb_default_dlt_job = dg.define_asset_job(
    name="igdb_default_dlt_job",
    selection=dg.AssetSelection.groups("dlt_assets_igdb_igdb_source"),
)

# Separate DLT job: popularity pipeline
igdb_popularity_dlt_job = dg.define_asset_job(
    name="igdb_popularity_dlt_job",
    selection=dg.AssetSelection.groups("dlt_assets_igdb_popularity_igdb_source"),
)

# Job for dbt assets (transformation)
dbt_transformation_job = dg.define_asset_job(
    name="dbt_transformation_job",
    selection=dg.AssetSelection.groups(
        "dbt_ingest"
    ),  # Selects assets from this component
)


# Full pipeline job: runs DLT assets (default + popularity) then DBT assets
full_pipeline_job = dg.define_asset_job(
    name="full_pipeline_job",
    selection=dg.AssetSelection.groups(
        "dlt_assets_igdb_igdb_source",
        "dlt_assets_igdb_popularity_igdb_source",
        "dbt_ingest",
    ),
)
