with
    source as (select * from {{ ref("stg_release_dates") }}),

    renamed as (

        select
            id,
            created_at,
            date,
            game,
            human,
            m,
            platform,
            updated_at,
            y,
            checksum,
            status,
            date_format,
            release_region

        from source

    )

select *
from renamed
