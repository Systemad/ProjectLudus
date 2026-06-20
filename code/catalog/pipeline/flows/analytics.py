from ingestions.steam.concurrent_users import steam_ccu
from prefect import flow, task
from prefect.concurrency.sync import concurrency

# from ingestions.umami.pageviews import run as run_umami


# @task
# def load_umami():
#    run_umami()


@task
def load_steam_ccu():
    steam_ccu()


@flow(log_prints=True)
def steam_ccu_pipeline():
    with concurrency("pipeline-lock", occupy=1):
        load_steam_ccu()
        # load_umami()


if __name__ == "__main__":
    steam_ccu_pipeline()
