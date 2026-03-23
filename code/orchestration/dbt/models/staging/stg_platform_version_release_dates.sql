with
    source as (

        select * from {{ source("igdb_source", "platform_version_release_dates") }}

    ),

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
            release_region,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
