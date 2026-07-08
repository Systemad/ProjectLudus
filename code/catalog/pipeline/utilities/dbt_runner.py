from pathlib import Path

from prefect_dbt import PrefectDbtRunner, PrefectDbtSettings


def run_dbt(select: str):
    dbt_project_dir = Path(__file__).resolve().parent.parent / "dbt"
    settings = PrefectDbtSettings(
        project_dir=dbt_project_dir,
        profiles_dir=dbt_project_dir,
    )
    PrefectDbtRunner(settings=settings).invoke(["build", "--select", select])
