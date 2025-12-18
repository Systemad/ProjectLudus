with
    source as (select * from {{ ref("stg_character_mug_shots") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            height,
            image_id,
            url,
            width,
            checksum,
            alpha_channel,
            animated

        from source

    )

select *
from renamed
