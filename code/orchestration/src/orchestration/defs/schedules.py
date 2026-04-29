import dagster as dg

from .jobs import igdb_dlt_job

pipeline_schedule = dg.ScheduleDefinition(
    name="daily_pipeline",
    cron_schedule="0 10 * * *",
    target=igdb_dlt_job,
    execution_timezone="UTC",
)
