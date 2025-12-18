with
    source as (select * from {{ ref("stg_game_engines") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            slug,
            url,
            checksum,
            description,
            logo__id,
            logo__alpha_channel,
            logo__animated,
            logo__height,
            logo__image_id,
            logo__url,
            logo__width,
            logo__checksum

        from source

    )

select *
from renamed
