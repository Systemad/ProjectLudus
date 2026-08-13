from ingestions.steam.reviews import run as run_steam_reviews
from ingestions.steam.store_details import run as run_steam_details
from ingestions.steam.store_pricing import run as run_steam_pricing
from prefect import flow, task
from prefect.concurrency.sync import concurrency
from flows.steam_tasks import dbt_build_steam


@task
def load_steam_details():
    run_steam_details()


@task
def load_steam_reviews():
    run_steam_reviews()


@task
def load_steam_pricing():
    run_steam_pricing()


@flow
def steam_ingestion_phase():
    """Fetch Steam data sequentially (rate-limited)."""
    load_steam_details()
    load_steam_pricing()
    load_steam_reviews()


@flow(log_prints=True)
def steam_details_pipeline():
    with concurrency("pipeline-lock", occupy=1):
        steam_ingestion_phase()
        dbt_build_steam()


if __name__ == "__main__":
    steam_details_pipeline()
