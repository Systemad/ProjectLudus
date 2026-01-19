{{ config(materialized="view") }}

{{
    dbt_utils.union_relations(
        relations=[
            ref("stg_pop_igdb_played"),
            ref("stg_pop_igdb_playing"),
            ref("stg_pop_igdb_visits"),
            ref("stg_pop_igdb_wants_to_play"),
            ref("stg_pop_steam_24hr_hours_watched"),
            ref("stg_pop_steam_24hr_peak_players"),
            ref("stg_pop_steam_global_top_sellers"),
            ref("stg_pop_steam_most_wishlisted_upcoming"),
            ref("stg_pop_steam_negative_reviews"),
            ref("stg_pop_steam_positive_reviews"),
            ref("stg_pop_steam_total_reviews"),
        ],
        exclude=["_dlt_id", "_dlt_load_id", "popularity_source"],
    )
}}
