{{ config(materialized="table") }}

select c.id as company_id, cd.value as game_id
from {{ ref("companies") }} c
join {{ ref("companies__developed") }} cd on cd._dlt_parent_id = c._dlt_id
