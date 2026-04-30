import dagster as dg

from .jobs import (
    dbt_transformation_job,
    igdb_dlt_job,
    make_typesense_index_job,
)


@dg.run_status_sensor(
    run_status=dg.DagsterRunStatus.SUCCESS,
    request_job=dbt_transformation_job,
)
def after_igdb_dlt_success(context: dg.RunStatusSensorContext):
    if context.dagster_run.job_name != igdb_dlt_job.name:
        return dg.SkipReason("Waiting for igdb_dlt_job success.")

    return dg.RunRequest(run_key=f"{context.dagster_run.run_id}:dbt")


@dg.run_status_sensor(
    run_status=dg.DagsterRunStatus.SUCCESS,
    request_job=make_typesense_index_job,
)
def after_dbt_transformation_success(context: dg.RunStatusSensorContext):
    if context.dagster_run.job_name != dbt_transformation_job.name:
        return dg.SkipReason("Waiting for dbt_transformation_job success.")

    return dg.RunRequest(run_key=f"{context.dagster_run.run_id}:typesense")
