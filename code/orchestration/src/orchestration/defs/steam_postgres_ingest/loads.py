import dlt
import requests

"""
@dlt.source
def my_source():
    @dlt.resource
    def hello_world():
        yield "hello, world!"

    return hello_world


my_load_source = my_source()
my_load_pipeline = dlt.pipeline(destination="postgres")
"""


@dlt.source
def steam_pipeline_source():
    # 1. Resource to fetch game_ids from your Postgres source table
    @dlt.resource(write_disposition="append")
    def game_ids():
        # Replace with your actual DB fetching logic
        # Example: cursor.execute("SELECT game_id FROM popularity_primitives WHERE id = 9")
        yield from [440, 570, 730]  # Mock IDs: TF2, Dota 2, CS2

    # 2. Transformer to fetch Steam data for each ID
    @dlt.transformer(data_from=game_ids)
    def steam_player_count(game_id):
        url = (
            "https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/"
        )
        params = {"appid": game_id}

        response = requests.get(url, params=params)
        response.raise_for_status()

        data = response.json().get("response", {})
        if "player_count" in data:
            yield {
                "game_id": game_id,
                "player_count": data["player_count"],
            }

    return game_ids, steam_player_count


pipeline = dlt.pipeline(
    pipeline_name="steam_analytics",
    destination="postgres",
    dataset_name="steam_data",
)

load_info = pipeline.run(steam_pipeline_source())
