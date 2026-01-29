with
    source as (select * from {{ ref("stg_event_logos") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            event,
            alpha_channel,
            animated,
            height,
            image_id,
            url,
            width,
            checksum

        from source

    )

select *
from renamed
