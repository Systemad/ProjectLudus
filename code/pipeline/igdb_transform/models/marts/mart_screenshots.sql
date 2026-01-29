with
    source as (select * from {{ ref("stg_screenshots") }}),

    renamed as (

        select id, alpha_channel, animated, game, height, image_id, url, width, checksum
        from source

    )

select *
from renamed
