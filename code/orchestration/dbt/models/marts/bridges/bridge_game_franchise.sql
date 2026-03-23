{{ config(materialized="table") }}

select g.id as game_id, t.value as franchise_id
from {{ ref("stg_games__franchises") }} t
inner join {{ ref("stg_games") }} g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_games") }} mg on g.id = mg.id
inner join {{ ref("mart_franchises") }} mf on t.value = mf.id
