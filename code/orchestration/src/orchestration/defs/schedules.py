import dagster as dg

from .jobs import igdb_popularity_dlt_job

pipeline_schedule = dg.ScheduleDefinition(
    name="daily_pipeline",
    cron_schedule="0 7 * * *",  # 7 AM daily
    target=igdb_popularity_dlt_job,
    execution_timezone="UTC",
)
