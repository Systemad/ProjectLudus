from ingestions.igdb.full_data import run as run_igdb_default
from ingestions.igdb.reference_data import run as run_ref_data
from ingestions.typesense.indexing import run_companies, run_games
from prefect import flow, task
from prefect.concurrency.sync import concurrency
from utilities.dbt_runner import run_dbt


@task
def typesense_games():
    run_games()


@task
def typesense_companies():
    run_companies()


@task
def dlt_igdb_default():
    run_igdb_default()


@task
def dlt_ref_data():
    run_ref_data()


@task
def dbt_build_igdb():
    run_dbt("+marts.gaming")


@flow
def igdb_load_phase():
    """Fetch IGDB data incrementally, run reference data, build dbt marts."""
    dlt_igdb_default()
    dlt_ref_data()
    dbt_build_igdb()


@flow
def igdb_index_phase():
    """Index marts into Typesense search."""
    typesense_games()
    typesense_companies()


@flow(log_prints=True)
def igdb_pipeline():
    with concurrency("pipeline-lock", occupy=1):
        igdb_load_phase()
        igdb_index_phase()


if __name__ == "__main__":
    igdb_pipeline()
