from pathlib import Path

import dagster as dg
from dagster import load_from_defs_folder

from orchestration.defs.jobs import (
    dbt_transformation_job,
    full_pipeline_job,
    gameclickcount_copy_job,
    igdb_default_dlt_job,
    igdb_popularity_dlt_job,
    make_typesense_index_job,
)
from orchestration.defs.schedules import pipeline_schedule
from orchestration.defs.sensors import (
    after_dbt_transformation_success,
    after_gameclickcount_copy_success,
    after_igdb_default_success,
    after_igdb_popularity_success,
)

defs = dg.Definitions.merge(
    load_from_defs_folder(
        path_within_project=Path(__file__).parent
    ),  # Loads your components
    dg.Definitions(
        jobs=[
            igdb_default_dlt_job,
            igdb_popularity_dlt_job,
            dbt_transformation_job,
            make_typesense_index_job,
            gameclickcount_copy_job,
            full_pipeline_job,
        ],
        schedules=[pipeline_schedule],
        sensors=[
            after_igdb_popularity_success,
            after_igdb_default_success,
            after_gameclickcount_copy_success,
            after_dbt_transformation_success,
        ],
    ),
)
