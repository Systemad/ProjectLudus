with
    source as (select * from {{ source("igdb_source_20251231072127", "platforms") }}),

    renamed as (

        select
            platform_family__id as id,
            id as platform_id,
            platform_family__name as name,
            platform_family__slug as slug,
            platform_family__checksum as checksum

        from source

    )

select *
from renamed
where id is not null
