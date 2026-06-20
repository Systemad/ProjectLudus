{{ config(materialized="table") }}

select
    g.id as game_engine_id,
    t.value as company_id
from {{ ref("stg_game_engines__companies") }} as t
inner join {{ ref("stg_game_engines") }} as g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_game_engines") }} as me on g.id = me.id
inner join {{ ref("mart_companies") }} as c on t.value = c.id
