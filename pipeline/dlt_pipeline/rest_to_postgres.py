import time

import dlt
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


#            {"name": "popularity_primitives", "table_name": "pop_prims"},
#
#             {"name": "player_perspectives", "table_name": "player_persp"},
# , max_table_nesting=0
# , max_table_nesting=1
#   max_table_nesting=3
@dlt.source(name="igdb", max_table_nesting=0)
def igdb_source():
    config: RESTAPIConfig = {
        "client": {
            "base_url": Endpoints.BASE_URL,
            "headers": {"Client-Id": IGDB_CLIENT_ID},
            "auth": ({"type": "bearer", "token": IGDB_ACCESS_TOKEN}),
            "paginator": OffsetPaginator(
                offset_param="offset",
                limit=500,
                total_path=None,
                stop_after_empty_page=True,
            ),
        },
        "resource_defaults": {
            "primary_key": "id",
            # "write_disposition": "merge",
            "endpoint": {"method": "POST", "params": {"fields": "*", "limit": 500}},
            "columns": {
                "id": {"data_type": "bigint"},
                "created_at": {"data_type": "bigint"},
                "updated_at": {"data_type": "bigint"},
            },
        },
        "resources": [
            Endpoints.AGE_RATINGS,
            Endpoints.AGE_RATING_CATEGORIES,
            Endpoints.AGE_RATING_CONTENT_DESCRIPTION_TYPES,
            Endpoints.AGE_RATING_CONTENT_DESCRIPTIONS_V2,
            Endpoints.AGE_RATING_ORGANIZATIONS,
            Endpoints.ARTWORKS,
            Endpoints.ALTERNATIVE_NAMES,
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
            Endpoints.COMPANIES,
            Endpoints.COMPANY_LOGOS,
            Endpoints.COMPANY_STATUSES,
            Endpoints.COMPANY_WEBSITES,
            Endpoints.COVERS,
            Endpoints.DATE_FORMATS,
            Endpoints.EVENTS,
            Endpoints.EVENT_LOGOS,
            Endpoints.EVENT_NETWORKS,
            Endpoints.EXTERNAL_GAMES,
            Endpoints.EXTERNAL_GAME_SOURCES,
            Endpoints.FRANCHISES,
            Endpoints.GAMES,
            Endpoints.GAME_ENGINES,
            Endpoints.GAME_ENGINE_LOGOS,
            Endpoints.GAME_LOCALIZATIONS,
            Endpoints.GAME_MODES,
            Endpoints.GAME_RELEASE_FORMATS,
            Endpoints.GAME_STATUSES,
            Endpoints.GAME_TIME_TO_BEATS,
            Endpoints.GAME_TYPES,
            Endpoints.GAME_VERSIONS,
            Endpoints.GAME_VERSION_FEATURES,
            Endpoints.GAME_VERSION_FEATURE_VALUES,
            Endpoints.GAME_VIDEOS,
            Endpoints.GENRES,
            Endpoints.INVOLVED_COMPANIES,
            Endpoints.KEYWORDS,
            Endpoints.LANGUAGES,
            Endpoints.LANGUAGE_SUPPORTS,
            Endpoints.LANGUAGE_SUPPORT_TYPES,
            Endpoints.MULTIPLAYER_MODES,
            Endpoints.NETWORK_TYPES,
            Endpoints.PLATFORMS,
            Endpoints.PLATFORM_TYPES,
            Endpoints.PLATFORM_FAMILY,
            Endpoints.PLATFORM_VERSIONS,
            Endpoints.PLATFORM_VERSION_COMPANIES,
            Endpoints.PLATFORM_VERSION_RELEASE_DATES,
            Endpoints.PLATFORM_WEBSITES,
            Endpoints.PLAYER_PERSPECTIVES,
            # Endpoints.POPULARITY_PRIMITIVES,
            # Endpoints.POPULARITY_TYPES,
            Endpoints.REGIONS,
            Endpoints.RELEASE_DATES,
            Endpoints.RELEASE_DATE_REGIONS,
            Endpoints.RELEASE_DATE_STATUSES,
            Endpoints.SCREENSHOTS,
            Endpoints.THEMES,
            Endpoints.WEBSITES,
            Endpoints.WEBSITE_TYPES,
        ],
    }

    for data in rest_api_resources(config):
        time.sleep(0.2)
        yield data


def load_igdb():
    pipeline = dlt.pipeline(
        pipeline_name="igdb_dlt_pipeline",
        destination="postgres",
        dataset_name="igdb_raw",
        progress="alive_progress",
    )
    load_info = pipeline.run(igdb_source())

    print(load_info)


if __name__ == "__main__":
    load_igdb()
