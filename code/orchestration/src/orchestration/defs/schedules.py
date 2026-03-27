import dagster as dg

from .jobs import full_pipeline_job

pipeline_schedule = dg.ScheduleDefinition(
    name="daily_pipeline",
    cron_schedule="0 7 * * *",  # 7 AM daily
    target=full_pipeline_job,
    execution_timezone="UTC",
)
