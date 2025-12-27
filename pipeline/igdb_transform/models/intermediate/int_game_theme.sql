{{ config(materialized="view") }}

select g.id as game_id, t.value as theme_id
from {{ ref("stg_games__themes") }} t
inner join {{ ref("stg_games") }} g on t._dlt_parent_id = g._dlt_id
-- Filter against final marts
inner join {{ ref("mart_games") }} mg on g.id = mg.id
inner join {{ ref("mart_themes") }} mt on t.value = mt.id
