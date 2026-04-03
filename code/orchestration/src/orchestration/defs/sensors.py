import dagster as dg

from .jobs import (
    dbt_transformation_job,
    gameclickcount_copy_job,
    igdb_default_dlt_job,
    igdb_popularity_dlt_job,
    make_typesense_index_job,
)


@dg.run_status_sensor(
    run_status=dg.DagsterRunStatus.SUCCESS,
    request_job=igdb_default_dlt_job,
)
def after_igdb_popularity_success(context: dg.RunStatusSensorContext):
    if context.dagster_run.job_name != igdb_popularity_dlt_job.name:
        return dg.SkipReason("Waiting for igdb_popularity_dlt_job success.")

    return dg.RunRequest(run_key=f"{context.dagster_run.run_id}:igdb_default")


@dg.run_status_sensor(
    run_status=dg.DagsterRunStatus.SUCCESS,
    request_job=gameclickcount_copy_job,
)
def after_igdb_default_success(context: dg.RunStatusSensorContext):
    if context.dagster_run.job_name != igdb_default_dlt_job.name:
        return dg.SkipReason("Waiting for igdb_default_dlt_job success.")

    return dg.RunRequest(run_key=f"{context.dagster_run.run_id}:gameclickcount_copy")


@dg.run_status_sensor(
    run_status=dg.DagsterRunStatus.SUCCESS,
    request_job=dbt_transformation_job,
)
def after_gameclickcount_copy_success(context: dg.RunStatusSensorContext):
    if context.dagster_run.job_name != gameclickcount_copy_job.name:
        return dg.SkipReason("Waiting for gameclickcount_copy_job success.")

    return dg.RunRequest(run_key=f"{context.dagster_run.run_id}:dbt")


@dg.run_status_sensor(
    run_status=dg.DagsterRunStatus.SUCCESS,
    request_job=make_typesense_index_job,
)
def after_dbt_transformation_success(context: dg.RunStatusSensorContext):
    if context.dagster_run.job_name != dbt_transformation_job.name:
        return dg.SkipReason("Waiting for dbt_transformation_job success.")

    return dg.RunRequest(run_key=f"{context.dagster_run.run_id}:typesense")
