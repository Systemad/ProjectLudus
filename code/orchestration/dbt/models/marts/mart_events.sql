with
    source as (select * from {{ ref("stg_events") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            slug,
            event_logo,
            start_time as start_time_epoch,
            to_timestamp(start_time::numeric) as start_time_utc,
            time_zone,
            live_stream_url,
            checksum,
            end_time as end_time_epoch,
            to_timestamp(end_time::numeric) as end_time_utc,
            description

        from source

    )

select *
from renamed
