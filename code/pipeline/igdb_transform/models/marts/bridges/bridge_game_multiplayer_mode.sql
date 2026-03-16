{{ config(materialized="table") }}

select distinct g.id as game_id, mm.id as multiplayer_mode_id
from {{ ref("stg_games__multiplayer_modes") }} t
inner join {{ ref("stg_games") }} g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_games") }} mg on g.id = mg.id
inner join {{ ref("mart_multiplayer_modes") }} mm on t.id = mm.id and mm.game = g.id
where mm.id is not null
