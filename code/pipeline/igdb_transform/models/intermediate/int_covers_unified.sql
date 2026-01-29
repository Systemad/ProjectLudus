with
    all_covers as (
        select
            id, game_id, alpha_channel, animated, height, image_id, url, width, checksum
        from {{ ref("stg_covers") }}
        union
        select
            id, game_id, alpha_channel, animated, height, image_id, url, width, checksum
        from {{ ref("stg_game_localization_cover") }}
    )

select *
from all_covers
where id is not null and id != 0
