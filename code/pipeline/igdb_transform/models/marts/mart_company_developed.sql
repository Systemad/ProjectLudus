{{ config(materialized="table") }}

select c.id as company_id, g.id as game_id
from {{ ref("stg_companies__developed") }} t
join {{ ref("stg_companies") }} c on t._dlt_parent_id = c._dlt_id
join {{ ref("stg_games") }} g on t.value = g.id

inner join {{ ref("mart_companies") }} mc on c.id = mc.id
inner join {{ ref("mart_games") }} mg on g.id = mg.id
