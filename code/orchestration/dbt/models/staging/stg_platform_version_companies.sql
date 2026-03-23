with
    source as (select * from {{ source("igdb_source", "platform_version_companies") }}),

    renamed as (

        select
            id,
            company,
            developer,
            manufacturer,
            checksum,
            _dlt_load_id,
            _dlt_id,
            comment

        from source

    )

select *
from renamed
