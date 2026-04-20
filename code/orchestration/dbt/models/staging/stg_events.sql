with
    source as (select * from {{ source("igdb_source", "events") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            slug,
            event_logo,
            start_time,
            time_zone,
            live_stream_url,
            checksum,
            _dlt_load_id,
            _dlt_id,
            end_time,
            description

        from source

    )

select *
from renamed
