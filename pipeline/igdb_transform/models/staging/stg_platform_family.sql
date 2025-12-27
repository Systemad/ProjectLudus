with
    source as (select * from {{ source("igdb_raw_v2", "platforms") }}),

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
