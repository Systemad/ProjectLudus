{{ config(materialized="view") }}

with
    base_games as (
        select id, _dlt_id from {{ ref("stg_games") }} where parent_game is null
    ),

    dlc_links as (
        select bg.id as base_game_id, t.value::bigint as dlc_game_id
        from {{ ref("stg_games__dlcs") }} t
        inner join base_games bg on t._dlt_parent_id = bg._dlt_id
    )

select *
from dlc_links
