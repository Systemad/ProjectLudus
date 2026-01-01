import time

import dlt
from dlt.sources.helpers.rest_client.auth import BearerTokenAuth
from dlt.sources.helpers.rest_client.paginators import OffsetPaginator
from dlt.sources.rest_api import (
    rest_api_resources,
)
from dlt.sources.rest_api.typing import (
    RESTAPIConfig,
)
from dotenv import load_dotenv
from endpoints import Endpoints

load_dotenv()

IGDB_CLIENT_ID = "i0s32q3oi8z074rvq0ljkaupnbkq98"
IGDB_ACCESS_TOKEN = "9c6ddwo8bno5qz5b06p9iq2xdi49un"


default: RESTAPIConfig = {
    "client": {
        "base_url": Endpoints.BASE_URL,
        "headers": {"Client-Id": dlt.secrets["igdb.clientid"]},
        "auth": BearerTokenAuth(dlt.secrets["igdb.token"]),
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
            },
        },
        # "columns": {
        #    "id": {"data_type": "bigint"},
        #    "created_at": {"data_type": "bigint"},
        #    "updated_at": {"data_type": "bigint"},
        # },
    },
    "resources": [
        Endpoints.AGE_RATING_CATEGORIES,
        Endpoints.AGE_RATING_CONTENT_DESCRIPTION_TYPES,
        Endpoints.AGE_RATING_CONTENT_DESCRIPTIONS_V2,
        Endpoints.AGE_RATING_ORGANIZATIONS,
        Endpoints.ARTWORK_TYPES,
        Endpoints.CHARACTERS,
        Endpoints.CHARACTER_GENDERS,
        Endpoints.CHARACTER_MUG_SHOTS,
        Endpoints.CHARACTER_SPECIES,
        Endpoints.COLLECTIONS,
        Endpoints.COLLECTION_MEMBERSHIPS,
        Endpoints.COLLECTION_MEMBERSHIP_TYPES,
        Endpoints.COLLECTION_RELATIONS,
        Endpoints.COLLECTION_RELATION_TYPES,
        Endpoints.COLLECTION_TYPES,
        {
            "max_table_nesting": 1,
            "name": Endpoints.COMPANIES,
            "endpoint": {
                "path": Endpoints.COMPANIES,
                "params": {
                    "fields": "*,logo.*,websites.*",
                },
            },
        },
        Endpoints.COMPANY_STATUSES,
        Endpoints.DATE_FORMATS,
        Endpoints.EVENTS,
        Endpoints.EVENT_LOGOS,
        Endpoints.EVENT_NETWORKS,
        Endpoints.EXTERNAL_GAME_SOURCES,
        Endpoints.FRANCHISES,
        Endpoints.GAME_MODES,
        Endpoints.GAME_RELEASE_FORMATS,
        Endpoints.GAME_STATUSES,
        Endpoints.GAME_TIME_TO_BEATS,
        Endpoints.GAME_TYPES,
        Endpoints.GAME_VERSIONS,
        Endpoints.GAME_VERSION_FEATURES,
        Endpoints.GAME_VERSION_FEATURE_VALUES,
        Endpoints.GENRES,
        Endpoints.KEYWORDS,
        Endpoints.LANGUAGES,
        Endpoints.LANGUAGE_SUPPORT_TYPES,
        Endpoints.NETWORK_TYPES,
        {
            "max_table_nesting": 1,
            "name": Endpoints.PLATFORMS,
            "endpoint": {
                "path": Endpoints.PLATFORMS,
                "params": {
                    "fields": "*,platform_family.*,platform_logo.*,platform_type.*",
                },
            },
        },
        {
            "max_table_nesting": 1,
            "name": Endpoints.PLATFORM_VERSIONS,
            "endpoint": {
                "path": Endpoints.PLATFORM_VERSIONS,
                "params": {
                    "fields": "*",
                },
            },
        },
        Endpoints.PLATFORM_VERSION_COMPANIES,
        Endpoints.PLATFORM_VERSION_RELEASE_DATES,
        Endpoints.PLAYER_PERSPECTIVES,
        Endpoints.REGIONS,
        Endpoints.RELEASE_DATE_REGIONS,
        Endpoints.RELEASE_DATE_STATUSES,
        Endpoints.THEMES,
        Endpoints.PLATFORM_WEBSITES,
        Endpoints.WEBSITE_TYPES,
        {
            "max_table_nesting": 1,
            "name": Endpoints.GAME_LOCALIZATIONS,
            "endpoint": {
                "path": Endpoints.GAME_LOCALIZATIONS,
                "params": {
                    "fields": "*,cover.*,region.*",
                },
            },
        },
        {
            "max_table_nesting": 1,
            "name": Endpoints.GAME_ENGINES,
            "endpoint": {
                "path": Endpoints.GAME_ENGINES,
                "params": {
                    "fields": "*,logo.*",
                },
            },
        },
        {
            "max_table_nesting": 1,
            "name": Endpoints.GAMES,
            "endpoint": {
                "path": Endpoints.GAMES,
                "params": {
                    "fields": "*,age_ratings.*,artworks.*,alternative_names.*,game_localizations.*,external_games.*,websites.*,release_dates.*,cover.*,screenshots.*,multiplayer_modes.*,language_supports.*,involved_companies.*,videos.*",
                },
            },
        },
    ],
}


@dlt.source(name="igdb", max_table_nesting=0)
def igdb_source():
    for data in rest_api_resources(default):
        time.sleep(0.2)
        yield data

        # Endpoints.POPULARITY_PRIMITIVES,
        # Endpoints.POPULARITY_TYPES,
        # Same as WEBSITES
        # Endpoints.AGE_RATINGS,
        # Endpoints.ALTERNATIVE_NAMES,
        # Endpoints.ARTWORKS,
        # Endpoints.EXTERNAL_GAMES,
        # Endpoints.GAME_LOCALIZATIONS,
        # Endpoints.GAME_VIDEOS,
        # Endpoints.INVOLVED_COMPANIES,
        # Endpoints.LANGUAGE_SUPPORTS,
        # Endpoints.MULTIPLAYER_MODES,
        # Endpoints.SCREENSHOTS,
        # Endpoints.COVERS,
        # Endpoints.RELEASE_DATES,
        # Endpoints.WEBSITES,


def load_igdb():
    pipeline = dlt.pipeline(
        pipeline_name="igdb_pipeline",
        destination="postgres",
        dataset_name="igdb_source",
        progress="alive_progress",
        dev_mode=False,
    )
    load_info = pipeline.run(igdb_source())

    print(load_info)


if __name__ == "__main__":
    load_igdb()
