with
    source as (select * from {{ ref("stg_platform_websites") }}),

    renamed as (

        select id, created_at, updated_at, trusted, url, checksum, type from source

    )

select *
from renamed
