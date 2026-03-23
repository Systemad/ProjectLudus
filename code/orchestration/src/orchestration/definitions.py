from pathlib import Path

import dagster as dg
from dagster import load_from_defs_folder

from .defs.jobs import (
    dbt_transformation_job,
    full_pipeline_job,
    igdb_default_dlt_job,
    igdb_ingestion_job,
    igdb_popularity_dlt_job,
)
from .defs.schedules import pipeline_schedule

defs = dg.Definitions.merge(
    load_from_defs_folder(
        path_within_project=Path(__file__).parent
    ),  # Loads your components
    dg.Definitions(
        jobs=[
            igdb_ingestion_job,
            igdb_default_dlt_job,
            igdb_popularity_dlt_job,
            dbt_transformation_job,
            full_pipeline_job,
        ],
        schedules=[pipeline_schedule],
    ),
)
