with
    source as (select * from {{ ref("stg_platform_version_release_dates") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            date,
            human,
            m,
            y,
            checksum,
            date_format,
            release_region

        from source

    )

select *
from renamed
