{{ config(materialized="table") }}

select t.value as company_id, g.id as platform_version_id
from {{ ref("stg_platform_versions__companies") }} t
-- Join to parent to get the real platform_version_id
inner join {{ ref("stg_platform_versions") }} g on t._dlt_parent_id = g._dlt_id
-- INNER JOIN ensures we only keep relationships where the company actually exists
inner join {{ ref("mart_companies") }} mc on t.value = mc.id
