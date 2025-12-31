with

    formatted as (

        select distinct
            id, alpha_channel, animated, game, height, image_id, url, width, checksum

        from {{ ref("stg_artworks") }}

    )

select *
from formatted
