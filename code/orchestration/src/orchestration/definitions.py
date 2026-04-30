from pathlib import Path

import dagster as dg
from dagster import load_from_defs_folder

from orchestration.defs.jobs import (
    dbt_transformation_job,
    full_pipeline_job,
    igdb_dlt_job,
    make_typesense_index_job,
    umami_job,
)
from orchestration.defs.schedules import pipeline_schedule
from orchestration.defs.sensors import (
    after_dbt_transformation_success,
    after_igdb_dlt_success,
)

defs = dg.Definitions.merge(
    load_from_defs_folder(path_within_project=Path(__file__).parent),
    dg.Definitions(
        jobs=[
            igdb_dlt_job,
            dbt_transformation_job,
            make_typesense_index_job,
            umami_job,
            full_pipeline_job,
        ],
        schedules=[pipeline_schedule],
        sensors=[
            after_igdb_dlt_success,
            after_dbt_transformation_success,
        ],
    ),
)
