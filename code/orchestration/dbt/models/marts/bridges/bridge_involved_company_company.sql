{{ config(materialized="table") }}

select
    ic.id as involved_company_id,
    ic.company as company_id
from {{ ref("mart_involved_companies") }} ic
inner join {{ ref("mart_companies") }} c on ic.company = c.id
where ic.company is not null
