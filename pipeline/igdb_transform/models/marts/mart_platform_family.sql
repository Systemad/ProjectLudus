with
    formatted as (

        select id, platform_id, name, slug, checksum
        from {{ ref("stg_platform_family") }}
    )

select *
from formatted
