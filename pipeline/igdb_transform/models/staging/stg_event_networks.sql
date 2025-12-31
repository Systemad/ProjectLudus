with
    source as (

        select * from {{ source("igdb_source_20251231072127", "event_networks") }}

    ),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            event,
            url,
            network_type,
            checksum,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
