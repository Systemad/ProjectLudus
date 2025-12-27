with
    formatted as (

        select id, platform_id, name, created_at, updated_at, checksum
        from {{ ref("stg_platform_type") }}
    )

select *
from formatted
