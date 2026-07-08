import logging

from ingestions.steam.reviews import run as run_steam_reviews
from ingestions.steam.store_details import run as run_steam_details
from ingestions.steam.store_pricing import run as run_steam_pricing
from prefect import flow, task
from prefect.concurrency.sync import concurrency
from utilities.database import get_connection
from utilities.dbt_runner import run_dbt

from flows.igdb import dlt_igdb_default
from flows.steam_game_index import (
    fetch_popularity_primitives,
    fetch_steam_app_ids,
    write_tracked_games,
)

logger = logging.getLogger(__name__)


@task
def load_steam_details():
    run_steam_details()


@task
def load_steam_reviews():
    run_steam_reviews()


@task
def load_steam_pricing():
    run_steam_pricing()


@task(retries=0)
def fill_igdb_gaps():
    conn = get_connection()
    with conn:
        rows = conn.execute("""
            SELECT t.game_id
            FROM steam.tracked_games t
            LEFT JOIN igdb_source.games g ON t.game_id = g.id
            WHERE g.id IS NULL
        """).fetchall()
    conn.close()
    if not rows:
        logger.info("No missing games — IGDB is up to date.")
        return
    logger.info("Missing %d games — triggering incremental IGDB pipeline", len(rows))
    dlt_igdb_default()


@task
def dbt_build_steam():
    run_dbt("+staging.steam")


@flow
def steam_game_index_phase():
    """Discover tracked games from IGDB popularity and fill gaps."""
    game_ids = fetch_popularity_primitives()
    if not game_ids:
        return
    results = fetch_steam_app_ids(game_ids)
    if not results:
        return
    write_tracked_games(results)
    fill_igdb_gaps()


@flow
def steam_ingestion_phase():
    """Fetch Steam data sequentially (rate-limited)."""
    load_steam_details()
    load_steam_pricing()
    load_steam_reviews()


@flow(log_prints=True)
def steam_details_pipeline():
    with concurrency("pipeline-lock", occupy=1):
        steam_game_index_phase()
        steam_ingestion_phase()
        dbt_build_steam()


if __name__ == "__main__":
    steam_details_pipeline()
