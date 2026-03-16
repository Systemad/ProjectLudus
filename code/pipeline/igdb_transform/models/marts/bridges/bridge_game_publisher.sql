{{ config(materialized="table") }}

select distinct ic.game as game_id, ic.company as company_id
from {{ ref("mart_involved_companies") }} ic
inner join {{ ref("mart_games") }} g on ic.game = g.id
inner join {{ ref("mart_companies") }} c on ic.company = c.id
where ic.publisher = true
