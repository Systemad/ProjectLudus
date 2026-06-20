import dlt
from dlt.sources.rest_api import (
    rest_api_source,
)
from utilities.igdb_client import IGDB__CLIENT_ID, get_igdb_auth
from utilities.paginator import IGDBPaginator

_INC_QUERY = "fields *; where updated_at > {incremental.start_value}; limit 500; sort updated_at desc;"
_NO_INC_QUERY = "fields *; limit 500;"

default = rest_api_source(
    name="igdb",
    config={
        "client": {
            "base_url": "https://api.igdb.com/v4/",
            "headers": {"Client-Id": IGDB__CLIENT_ID},
            "auth": get_igdb_auth(),
            "paginator": IGDBPaginator(limit=500),
        },
        "resource_defaults": {
            "primary_key": "id",
            "write_disposition": "merge",
            "max_table_nesting": 0,
            "endpoint": {
                "method": "POST",
                "data": _INC_QUERY,
                "incremental": {
                    "cursor_path": "updated_at",
                    "initial_value": 0,
                    "on_cursor_value_missing": "include",
                },
            },
        },
        "resources": [
            "age_rating_categories",
            "age_rating_content_description_types",
            "age_rating_content_descriptions_v2",
            "age_rating_organizations",
            "artwork_types",
            {
                "max_table_nesting": 1,
                "name": "characters",
                "endpoint": {
                    "path": "characters",
                    "data": _INC_QUERY,
                },
            },
            "character_genders",
            {
                "name": "character_mug_shots",
                "endpoint": {
                    "path": "character_mug_shots",
                    "method": "POST",
                    "data": _NO_INC_QUERY,
                    "incremental": None,
                },
            },
            "character_species",
            {
                "max_table_nesting": 1,
                "name": "collections",
                "endpoint": {
                    "path": "collections",
                    "data": _INC_QUERY,
                },
            },
            "collection_memberships",
            "collection_membership_types",
            "collection_relations",
            "collection_relation_types",
            {
                "max_table_nesting": 1,
                "name": "companies",
                "endpoint": {
                    "path": "companies",
                    "data": "fields *,logo.*,websites.*; where updated_at > {incremental.start_value}; limit 500; sort updated_at desc;",
                },
            },
            {
                "max_table_nesting": 1,
                "name": "events",
                "endpoint": {
                    "path": "events",
                    "data": _INC_QUERY,
                },
            },
            "event_logos",
            "event_networks",
            "external_game_sources",
            {
                "max_table_nesting": 1,
                "name": "franchises",
                "endpoint": {
                    "path": "franchises",
                    "data": _INC_QUERY,
                },
            },
            "game_time_to_beats",
            "keywords",
            {
                "max_table_nesting": 1,
                "name": "network_types",
                "endpoint": {
                    "path": "network_types",
                    "data": _INC_QUERY,
                },
            },
            {
                "max_table_nesting": 1,
                "name": "platforms",
                "endpoint": {
                    "path": "platforms",
                    "data": "fields *,platform_family.*,platform_logo.*,platform_type.*; where updated_at > {incremental.start_value}; limit 500; sort updated_at desc;",
                },
            },
            {
                "max_table_nesting": 1,
                "name": "platform_versions",
                "endpoint": {
                    "path": "platform_versions",
                    "data": _NO_INC_QUERY,
                    "incremental": None,
                },
            },
            {
                "name": "platform_version_companies",
                "endpoint": {
                    "path": "platform_version_companies",
                    "data": _NO_INC_QUERY,
                    "incremental": None,
                },
            },
            "platform_version_release_dates",
            {
                "name": "platform_websites",
                "endpoint": {
                    "path": "platform_websites",
                    "data": _NO_INC_QUERY,
                    "incremental": None,
                },
            },
            {
                "max_table_nesting": 1,
                "name": "game_localizations",
                "endpoint": {
                    "path": "game_localizations",
                    "data": "fields *,cover.*,region.*; where updated_at > {incremental.start_value}; limit 500; sort updated_at desc;",
                },
            },
            {
                "max_table_nesting": 1,
                "name": "game_engines",
                "endpoint": {
                    "path": "game_engines",
                    "data": "fields *,logo.*; where updated_at > {incremental.start_value}; limit 500; sort updated_at desc;",
                },
            },
            {
                "max_table_nesting": 2,
                "name": "games",
                "endpoint": {
                    "path": "games",
                    "data": "fields *,age_ratings.*,artworks.*,alternative_names.*,game_localizations.*,external_games.*,websites.*,release_dates.*,cover.*,screenshots.*,multiplayer_modes.*,language_supports.*,involved_companies.*,videos.*; where updated_at > {incremental.start_value}; limit 500; sort updated_at desc;",
                },
            },
        ],
    },
)


pipeline = dlt.pipeline(
    pipeline_name="igdb_pipeline",
    destination="postgres",
    dataset_name="igdb_source",
    progress="log",
    dev_mode=False,
)


def run():
    pipeline.run(default)


