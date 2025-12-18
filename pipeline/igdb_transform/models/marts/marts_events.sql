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
            start_time,
            time_zone,
            live_stream_url,
            games,
            checksum,
            end_time,
            description,
            videos,
            event_networks

        from source

    )

select *
from renamed
