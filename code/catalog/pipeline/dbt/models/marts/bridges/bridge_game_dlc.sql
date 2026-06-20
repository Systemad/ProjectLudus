{{ config(materialized="table") }}

with
base_games as (
    select
        sg.id,
        sg._dlt_id
    from {{ ref("stg_games") }} as sg
    inner join {{ ref("mart_games") }} as mg on sg.id = mg.id
    where sg.parent_game is null
),

dlc_links as (
    select
        bg.id as dlc_source_id,
        t.value::bigint as dlc_game_id
    from {{ ref("stg_games__dlcs") }} as t
    inner join base_games as bg on t._dlt_parent_id = bg._dlt_id
    inner join {{ ref("mart_games") }} as dlc_mg on t.value::bigint = dlc_mg.id
    where t.value is not null
)

select *
from dlc_links
