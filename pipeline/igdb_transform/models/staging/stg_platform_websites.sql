with
    source as (

        select * from {{ source("igdb_source_20251229083704", "platform_websites") }}

    ),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            trusted,
            url,
            checksum,
            type,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
