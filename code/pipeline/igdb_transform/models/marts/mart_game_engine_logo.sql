with
    formatted as (

        select
            id,
            game_engine_id,
            alpha_channel,
            animated,
            height,
            image_id,
            url,
            width,
            checksum
        from {{ ref("stg_game_engines_logos") }}
    )

select *
from formatted
