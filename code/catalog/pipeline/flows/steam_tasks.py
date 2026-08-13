from prefect import task

from utilities.dbt_runner import run_dbt


@task
def dbt_build_steam():
    run_dbt("+marts.steam")
