from pathlib import Path

from prefect_dbt import PrefectDbtRunner, PrefectDbtSettings

BASE_DIR = Path(__file__).resolve().parent.parent


def run_dbt(select: str):
    dbt_project_dir = BASE_DIR / "dbt"
    settings = PrefectDbtSettings(
        project_dir=dbt_project_dir,
        profiles_dir=dbt_project_dir,
    )
    PrefectDbtRunner(settings=settings).invoke(["build", "--select", select])
