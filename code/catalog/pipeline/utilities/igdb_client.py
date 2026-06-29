import os

from dlt.sources.helpers.rest_client.auth import BearerTokenAuth

# IGDB__CLIENT_ID = os.environ["IGDB__CLIENT_ID"]
# IGDB__ACCESS_TOKEN = os.environ["IGDB__ACCESS_TOKEN"]


# def get_igdb_auth() -> BearerTokenAuth:
#    return BearerTokenAuth(token=IGDB__ACCESS_TOKEN)


def get_igdb_client_id() -> str:
    return os.environ["IGDB__CLIENT_ID"]


def get_igdb_access_token() -> str:
    return os.environ["IGDB__ACCESS_TOKEN"]


def get_igdb_auth() -> BearerTokenAuth:
    access_token = os.environ["IGDB__ACCESS_TOKEN"]
    return BearerTokenAuth(token=access_token)


def get_igdb_headers() -> dict[str, str]:
    client_id = os.environ["IGDB__CLIENT_ID"]
    access_token = os.environ["IGDB__ACCESS_TOKEN"]
    return {
        "Client-Id": client_id,
        "Authorization": f"Bearer {access_token}",
        "Content-Type": "text/plain",
    }


# def get_igdb_headers() -> dict[str, str]:
#    return {
#        "Client-Id": IGDB__CLIENT_ID,
#        "Authorization": f"Bearer {IGDB__ACCESS_TOKEN}",
#        "Content-Type": "text/plain",
#    }
