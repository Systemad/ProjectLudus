import os

import dlt
from dlt.common.schema import TColumnSchema
from dlt.sources.helpers.rest_client.auth import BearerTokenAuth
from dlt.sources.helpers.rest_client.paginators import OffsetPaginator
from dlt.sources.rest_api import (
    rest_api_source,
)
from endpoints import Endpoints

dlt_table_schema: dict[str, TColumnSchema] = {
    "updated_at": {"data_type": "bigint", "dedup_sort": "desc"}
}

popularity = rest_api_source(
    name="igdb_popularity",
    config={
        "client": {
            "base_url": Endpoints.BASE_URL,
            "headers": {"Client-Id": dlt.secrets["igdb.clientid"]},
            "auth": BearerTokenAuth(dlt.secrets["igdb.token"]),
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
            # {
            #    "name": Endpoints.POPULARITY_TYPES,
            #    "endpoint": {
            #        "path": Endpoints.POPULARITY_TYPES,
            #        "data": {
            #            "limit": 250,
            #        },
            #    },
            # },
            {
                "name": "pop_igdb_visits",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 1; limit 100;",
                },
            },
            {
                "name": "pop_igdb_playing",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 3; limit 100;",
                },
            },
            {
                "name": "pop_igdb_wants_to_play",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 2; limit 100;",
                },
            },
            {
                "name": "pop_igdb_played",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 4; limit 100;",
                },
            },
            {
                "name": "pop_steam_total_reviews",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 8; limit 100;",
                },
            },
            {
                "name": "pop_steam_negative_reviews",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 7; limit 100;",
                },
            },
            {
                "name": "pop_steam_24hr_peak_players",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 5; limit 100;",
                },
            },
            {
                "name": "pop_steam_positive_reviews",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 6; limit 100;",
                },
            },
            {
                "name": "pop_steam_most_wishlisted_upcoming",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 10; limit 100;",
                },
            },
            {
                "name": "pop_steam_global_top_sellers",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 9; limit 100;",
                },
            },
            {
                "name": "pop_steam_24hr_hours_watched",
                "endpoint": {
                    "method": "POST",
                    "path": Endpoints.POPULARITY_PRIMITIVES,
                    "data": "fields *; sort value desc; where popularity_type = 34; limit 100;",
                },
            },
        ],
    },
)
#                        "where": "popularity_type = (1,2,3,4,5,6,7,8)",
default = rest_api_source(
    name="igdb",
    config={
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
            Endpoints.POPULARITY_TYPES,
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
    },
)


# @dlt.source(name="igdb2", max_table_nesting=0)
# def igdb_source():
#    for data in rest_api_resources(default):
#        time.sleep(0.2)
#        yield data


#    for data in rest_api_resources(non_incremental):
#        time.sleep(0.2)
#        yield data


def load_igdb():
    pipeline = dlt.pipeline(
        pipeline_name="igdb_pipeline",
        destination="postgres",
        dataset_name="igdb_source",
        progress="alive_progress",
        dev_mode=False,
    )

    load_info_pop = pipeline.run(popularity)
    load_info_default = pipeline.run(default)

    print(load_info_pop)
    print(load_info_default)

    dbt = dlt.dbt.package(
        pipeline=pipeline,
        package_location="../igdb_transform",
    )
    run_params: list[str] = (
        ["--full-refresh"] if os.environ.get("DBT_FULL_REFRESH") else []
    )
    models = dbt.run_all(run_params=run_params)
    for model in models:
        print(model)


if __name__ == "__main__":
    load_igdb()
