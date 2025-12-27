with
    formatted as (

        select
            id, game_id, alpha_channel, animated, height, image_id, url, width, checksum
        from {{ ref("stg_covers") }}
    )

select *
from formatted
