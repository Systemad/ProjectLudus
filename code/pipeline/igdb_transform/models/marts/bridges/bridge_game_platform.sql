{{ config(materialized="table") }}

select distinct g.id as game_id, t.value as platform_id
from {{ ref("stg_games__platforms") }} t
inner join {{ ref("stg_games") }} g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_games") }} mg on g.id = mg.id
inner join {{ ref("mart_platforms") }} mp on t.value = mp.id
where t.value is not null
