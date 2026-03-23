with
    formatted as (

        select
            id, game_id, alpha_channel, animated, height, image_id, url, width, checksum
        from {{ ref("int_covers_unified") }}
        where id is not null
    )

select *
from formatted
