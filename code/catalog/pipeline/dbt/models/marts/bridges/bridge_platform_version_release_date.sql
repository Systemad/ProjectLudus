{{ config(materialized="table") }}

select
    t.value as release_date,
    g.id as platform_version
from {{ ref("stg_platform_versions__platform_version_release_dates") }} as t
inner join
    {{ ref("stg_platform_versions") }} as g
    on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_platform_versions") }} as mpv on g.id = mpv.id
inner join {{ ref("mart_platform_version_release_dates") }} as mpd on t.value = mpd.id