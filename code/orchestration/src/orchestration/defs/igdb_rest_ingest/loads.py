import os

import dlt
from dlt.sources.helpers.rest_client.auth import BearerTokenAuth
from dlt.sources.helpers.rest_client.paginators import OffsetPaginator
from dlt.sources.rest_api import (
    rest_api_source,
)
from dlt_typesense import typesense
from dotenv import load_dotenv

from .endpoints import Endpoints

load_dotenv()

client_id = os.getenv("IGDB_CLIENT_ID")
access_token = os.getenv("IGDB_ACCESS_TOKEN")

if not client_id or not access_token:
    raise ValueError(
        "Missing required environment variables: IGDB_CLIENT_ID and/or IGDB_ACCESS_TOKEN"
    )


popularity = rest_api_source(
    name="igdb_popularity",
    config={
        "client": {
            "base_url": Endpoints.BASE_URL,
            "headers": {"Client-Id": client_id},
            "auth": BearerTokenAuth(token=access_token),
        },
        "resource_defaults": {
            "primary_key": "id",
            "write_disposition": "append",
            "max_table_nesting": 0,
            "endpoint": {
                "method": "POST",
            },
        },
        "resources": [
            {
                "name": "pop_igdb_visits",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 1; limit 100;",
                },
            },
            {
                "name": "pop_igdb_playing",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 3; limit 100;",
                },
            },
            {
                "name": "pop_igdb_wants_to_play",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 2; limit 100;",
                },
            },
            {
                "name": "pop_igdb_played",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 4; limit 100;",
                },
            },
            {
                "name": "pop_steam_total_reviews",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 8; limit 100;",
                },
            },
            {
                "name": "pop_steam_negative_reviews",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 7; limit 100;",
                },
            },
            {
                "name": "pop_steam_24hr_peak_players",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 5; limit 100;",
                },
            },
            {
                "name": "pop_steam_positive_reviews",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 6; limit 100;",
                },
            },
            {
                "name": "pop_steam_most_wishlisted_upcoming",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 10; limit 100;",
                },
            },
            {
                "name": "pop_steam_global_top_sellers",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 9; limit 100;",
                },
            },
            {
                "name": "pop_steam_24hr_hours_watched",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES.value,
                    "data": "fields *; sort value desc; where popularity_type = 34; limit 100;",
                },
            },
        ],
    },
)

default = rest_api_source(
    name="igdb",
    config={
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
                # "data": "fields *; where updated_at > {incremental.start_value}; limit 500;",
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
            Endpoints.AGE_RATING_CATEGORIES.value,
            Endpoints.AGE_RATING_CONTENT_DESCRIPTION_TYPES.value,
            Endpoints.AGE_RATING_CONTENT_DESCRIPTIONS_V2.value,
            Endpoints.AGE_RATING_ORGANIZATIONS.value,
            Endpoints.ARTWORK_TYPES.value,
            Endpoints.CHARACTERS.value,
            Endpoints.CHARACTER_GENDERS.value,
            Endpoints.CHARACTER_MUG_SHOTS.value,
            Endpoints.CHARACTER_SPECIES.value,
            Endpoints.COLLECTIONS.value,
            Endpoints.COLLECTION_MEMBERSHIPS.value,
            Endpoints.COLLECTION_MEMBERSHIP_TYPES.value,
            Endpoints.COLLECTION_RELATIONS.value,
            Endpoints.COLLECTION_RELATION_TYPES.value,
            Endpoints.COLLECTION_TYPES.value,
            {
                "max_table_nesting": 1,
                "name": Endpoints.COMPANIES.value,
                "endpoint": {
                    "path": Endpoints.COMPANIES.value,
                    "params": {
                        "fields": "*,logo.*,websites.*",
                    },
                },
            },
            Endpoints.COMPANY_STATUSES.value,
            Endpoints.DATE_FORMATS.value,
            Endpoints.EVENTS.value,
            Endpoints.EVENT_LOGOS.value,
            Endpoints.EVENT_NETWORKS.value,
            Endpoints.EXTERNAL_GAME_SOURCES.value,
            Endpoints.FRANCHISES.value,
            Endpoints.GAME_MODES.value,
            Endpoints.GAME_RELEASE_FORMATS.value,
            Endpoints.GAME_STATUSES.value,
            Endpoints.GAME_TIME_TO_BEATS.value,
            Endpoints.GAME_TYPES.value,
            Endpoints.GAME_VERSIONS.value,
            Endpoints.GAME_VERSION_FEATURES.value,
            Endpoints.GAME_VERSION_FEATURE_VALUES.value,
            Endpoints.GENRES.value,
            Endpoints.KEYWORDS.value,
            Endpoints.LANGUAGES.value,
            Endpoints.LANGUAGE_SUPPORT_TYPES.value,
            Endpoints.NETWORK_TYPES.value,
            {
                "max_table_nesting": 1,
                "name": Endpoints.PLATFORMS.value,
                "endpoint": {
                    "path": Endpoints.PLATFORMS.value,
                    "params": {
                        "fields": "*,platform_family.*,platform_logo.*,platform_type.*",
                    },
                },
            },
            {
                "max_table_nesting": 1,
                "name": Endpoints.PLATFORM_VERSIONS.value,
                "endpoint": {
                    "path": Endpoints.PLATFORM_VERSIONS.value,
                    "params": {
                        "fields": "*",
                    },
                },
            },
            Endpoints.PLATFORM_VERSION_COMPANIES.value,
            Endpoints.PLATFORM_VERSION_RELEASE_DATES.value,
            Endpoints.PLAYER_PERSPECTIVES.value,
            Endpoints.REGIONS.value,
            Endpoints.RELEASE_DATE_REGIONS.value,
            Endpoints.RELEASE_DATE_STATUSES.value,
            Endpoints.THEMES.value,
            Endpoints.PLATFORM_WEBSITES.value,
            Endpoints.POPULARITY_TYPES.value,
            Endpoints.WEBSITE_TYPES.value,
            {
                "max_table_nesting": 1,
                "name": Endpoints.GAME_LOCALIZATIONS.value,
                "endpoint": {
                    "path": Endpoints.GAME_LOCALIZATIONS.value,
                    "params": {
                        "fields": "*,cover.*,region.*",
                    },
                },
            },
            {
                "max_table_nesting": 1,
                "name": Endpoints.GAME_ENGINES.value,
                "endpoint": {
                    "path": Endpoints.GAME_ENGINES.value,
                    "params": {
                        "fields": "*,logo.*",
                    },
                },
            },
            {
                "max_table_nesting": 1,
                "name": Endpoints.GAMES.value,
                "endpoint": {
                    "path": Endpoints.GAMES.value,
                    "params": {
                        "fields": "*,age_ratings.*,artworks.*,alternative_names.*,game_localizations.*,external_games.*,websites.*,release_dates.*,cover.*,screenshots.*,multiplayer_modes.*,language_supports.*,involved_companies.*,videos.*",
                    },
                },
            },
        ],
    },
)


popularity_pipeline = dlt.pipeline(
    pipeline_name="igdb_popularity_pipeline",
    destination="postgres",
    dataset_name="igdb_source",
    progress="alive_progress",
    dev_mode=False,
)

default_pipeline = dlt.pipeline(
    pipeline_name="igdb_default_pipeline",
    destination="postgres",
    dataset_name="igdb_source",
    progress="alive_progress",
    dev_mode=False,
)

make_typesense_index_pipeline = dlt.pipeline(
    pipeline_name="make_typesense_index_pipeline",
    destination=typesense(),
    dataset_name="games_to_typesense_dataset",
    progress="alive_progress",
    dev_mode=False,
)


# my_load_source = my_source()
# my_load_pipeline = dlt.pipeline(destination="postgres")


# my_load_source = my_source()

# my_load_pipeline = dlt.pipeline(destination="postgres")

# """"
#            # {
#            #    "name": Endpoints.POPULARITY_TYPES,
#            #    "endpoint": {
#            #        "path": Endpoints.POPULARITY_TYPES,
#            #        "data": {
#            #            "limit": 250,
#            #        },
#            #    },
#            # },#

# """
