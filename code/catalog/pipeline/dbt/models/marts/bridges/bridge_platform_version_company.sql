{{ config(materialized="table") }}

select
    t.value as company_id,
    g.id as platform_version_id
from {{ ref("stg_platform_versions__companies") }} as t
inner join
    {{ ref("stg_platform_versions") }} as g
    on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_platform_versions") }} as mpv on g.id = mpv.id
inner join {{ ref("mart_companies") }} as mc on t.value = mc.id
