import dagster as dg

from .jobs import full_pipeline_job

# Single schedule that triggers the full pipeline (DLT jobs first, then DBT)
pipeline_schedule = dg.ScheduleDefinition(
    name="daily_pipeline",
    cron_schedule="0 2 * * *",  # 2 AM daily
    target=full_pipeline_job,
    execution_timezone="UTC",
)
