import dlt
import requests
from dlt.sources.rest_api import (
    rest_api_source,
)
from utilities.igdb_client import get_igdb_auth, get_igdb_client_id, get_igdb_headers
from utilities.paginator import IGDBPaginator

_INC_QUERY = "fields *; where updated_at > {incremental.start_value}; limit 500; sort updated_at desc;"
_NO_INC_QUERY = "fields *; limit 500;"

_FIELDS_A = (
    "fields *,age_ratings.*,artworks.*,alternative_names.*,cover.*,"
    "multiplayer_modes.*,screenshots.*,videos.*; "
    "where updated_at > {cursor}; limit 500; offset {offset}; sort updated_at desc;"
)

_FIELDS_B = (
    "fields id,updated_at,game_localizations.*,external_games.*,"
    "involved_companies.*,language_supports.*,release_dates.*,websites.*; "
    "where updated_at > {cursor}; limit 500; offset {offset}; sort updated_at desc;"
)


def _make_source():
    from dlt.sources.rest_api.config_setup import register_paginator

    register_paginator("igdb_offset", IGDBPaginator)
    return rest_api_source(
        name="igdb",
        config={
            "client": {
                "base_url": "https://api.igdb.com/v4/",
                "headers": {"Client-Id": get_igdb_client_id()},
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
                    "write_disposition": "replace",
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
                    "write_disposition": "replace",
                    "endpoint": {
                        "path": "platform_versions",
                        "data": _NO_INC_QUERY,
                        "incremental": None,
                    },
                },
                {
                    "name": "platform_version_companies",
                    "write_disposition": "replace",
                    "endpoint": {
                        "path": "platform_version_companies",
                        "data": _NO_INC_QUERY,
                        "incremental": None,
                    },
                },
                "platform_version_release_dates",
                {
                    "name": "platform_websites",
                    "write_disposition": "replace",
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
            ],
        },
    )


def _make_igdb_session() -> requests.Session:
    session = requests.Session()
    session.headers.update(get_igdb_headers())
    return session


def _fetch_games(session, body_template, cursor, offset):
    body = body_template.format(cursor=cursor, offset=offset)
    response = session.post("https://api.igdb.com/v4/games", data=body)
    response.raise_for_status()
    return response.json()


@dlt.resource(
    name="games", primary_key="id", write_disposition="merge", max_table_nesting=2
)
def games_resource(
    cursor=dlt.sources.incremental(
        "updated_at", initial_value=0, on_cursor_value_missing="include"
    ),
):
    session = _make_igdb_session()
    start = cursor.start_value
    offset = 0

    while True:
        page_a = _fetch_games(session, _FIELDS_A, start, offset)
        page_b = _fetch_games(session, _FIELDS_B, start, offset)

        merged = {}
        for game in page_a:
            merged[game["id"]] = game
        for game in page_b:
            gid = game["id"]
            if gid in merged:
                merged[gid].update(game)
            else:
                merged[gid] = game

        yield list(merged.values())

        if len(page_a) < 500:
            break
        offset += 500


def run():
    igdb_source = _make_source()
    pipeline = dlt.pipeline(
        pipeline_name="igdb_pipeline",
        destination="postgres",
        dataset_name="igdb_source",
        progress="log",
        dev_mode=False,
    )
    pipeline.run(igdb_source)
    pipeline.run(games_resource)
