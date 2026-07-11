{{ config(materialized="table") }}

select
    c.id as company_id,
    t.company_type_id
from {{ ref("stg_companies__company_type_histories") }} as t
inner join {{ ref("stg_companies") }} as c on t._dlt_parent_id = c._dlt_id
inner join {{ ref("mart_companies") }} as mc on c.id = mc.id
