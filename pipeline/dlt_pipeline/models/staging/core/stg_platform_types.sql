{{ config(materialized="view") }}

with
    source as (select * from {{ source("igdb_source_20251229083704", "platforms") }}),
    renamed as (
        select
            platform_type__id as id,
            platform_type__name as name,
            platform_type__created_at as created_at,
            platform_type__updated_at as updated_at,
            platform_type__checksum as checksum
        from source
    )
select *
from renamed
