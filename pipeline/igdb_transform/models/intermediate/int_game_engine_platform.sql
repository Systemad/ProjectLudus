{{ config(materialized="view") }}

select g.id as game_engine_id, t.value as platform_id

from {{ ref("stg_game_engines__platforms") }} t

inner join {{ ref("stg_game_engines") }} g on t._dlt_parent_id = g._dlt_id
