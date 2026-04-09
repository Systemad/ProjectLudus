import os

from dlt.sources.helpers.rest_client.auth import BearerTokenAuth
from dlt.sources.helpers.rest_client.paginators import OffsetPaginator
from dlt.sources.rest_api import rest_api_source
from dlt.sources.rest_api.typing import RESTAPIConfig
from dotenv import load_dotenv

from .endpoints import Endpoints

load_dotenv()

client_id = os.getenv("IGDB_CLIENT_ID")
access_token = os.getenv("IGDB_ACCESS_TOKEN")

if not client_id or not access_token:
    raise ValueError(
        "Missing required environment variables: IGDB_CLIENT_ID and/or IGDB_ACCESS_TOKEN"
    )

DEBUG_SOURCE_NAME = "igdb_debug_incremental"


debug_incremental_config: RESTAPIConfig = {
    "client": {
        "base_url": Endpoints.BASE_URL.value,
        "headers": {"Client-Id": client_id},
        "auth": BearerTokenAuth(token=access_token),
        "paginator": OffsetPaginator(
            offset_param="offset",
            limit=500,
            total_path=None,
            stop_after_empty_page=True,
        ),
    },
    "resource_defaults": {
        "primary_key": "id",
        "write_disposition": "merge",
        "max_table_nesting": 0,
        "endpoint": {
            "method": "POST",
            "params": {
                "fields": "*",
                "where": "updated_at > {incremental.start_value}",
                "limit": 500,
            },
            "incremental": {
                "cursor_path": "updated_at",
                "initial_value": 0,
                "on_cursor_value_missing": "include",
            },
        },
    },
    "resources": [
        Endpoints.EVENTS.value,
        Endpoints.GAME_ENGINES.value,
    ],
}


debug_incremental_source = rest_api_source(
    name=DEBUG_SOURCE_NAME,
    config=debug_incremental_config,
)
