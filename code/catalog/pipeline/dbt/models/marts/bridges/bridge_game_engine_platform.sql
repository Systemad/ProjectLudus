{{ config(materialized="table") }}
select
    g.id as game_engine_id,
    t.value as platform_id
from {{ ref("stg_game_engines__platforms") }} as t
inner join {{ ref("stg_game_engines") }} as g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_game_engines") }} as me on g.id = me.id
inner join {{ ref("mart_platforms") }} as p on t.value = p.id
