{{ config(materialized="table") }}

select g.id as game_id, t.value as keyword_id
from {{ ref("stg_games__keywords") }} t
inner join {{ ref("stg_games") }} g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_keywords") }} e on t.value = e.id
