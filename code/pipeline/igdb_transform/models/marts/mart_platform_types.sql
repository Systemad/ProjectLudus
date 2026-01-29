with
    formatted as (

        select distinct id, name, created_at, updated_at, checksum
        from {{ ref("stg_platform_type") }}
    )

select *
from formatted
