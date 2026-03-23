{{ config(materialized="table") }}

select g.id as game_id, t.value as player_perspective_id
from {{ ref("stg_games__player_perspectives") }} t
inner join {{ ref("stg_games") }} g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_games") }} mg on g.id = mg.id
inner join {{ ref("mart_player_perspectives") }} mp on t.value = mp.id
