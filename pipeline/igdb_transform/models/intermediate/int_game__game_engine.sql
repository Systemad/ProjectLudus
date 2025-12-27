{{ config(materialized="view") }}

select g.id as game_id, t.value as game_engine_id

from {{ ref("stg_games__game_engines") }} t

inner join {{ ref("stg_games") }} g on t._dlt_parent_id = g._dlt_id
