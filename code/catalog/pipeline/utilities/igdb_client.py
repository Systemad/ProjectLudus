import os

from dlt.sources.helpers.rest_client.auth import BearerTokenAuth

IGDB__CLIENT_ID = os.environ["IGDB__CLIENT_ID"]
IGDB__ACCESS_TOKEN = os.environ["IGDB__ACCESS_TOKEN"]


def get_igdb_auth() -> BearerTokenAuth:
    return BearerTokenAuth(token=IGDB__ACCESS_TOKEN)


def get_igdb_client_id() -> str:
    return IGDB__CLIENT_ID


def get_igdb_access_token() -> str:
    return IGDB__ACCESS_TOKEN


def get_igdb_headers() -> dict[str, str]:
    return {
        "Client-Id": IGDB__CLIENT_ID,
        "Authorization": f"Bearer {IGDB__ACCESS_TOKEN}",
        "Content-Type": "text/plain",
    }
