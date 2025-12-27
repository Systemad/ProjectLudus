with
    source as (select * from {{ ref("stg_games") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            cover__id,
            cover__alpha_channel,
            cover__animated,
            cover__game,
            cover__height,
            cover__image_id,
            cover__url,
            cover__width,
            cover__checksum,
            name,
            parent_game,
            slug,
            summary,
            url,
            checksum,
            game_type,
            first_release_date,
            rating,
            rating_count,
            total_rating,
            total_rating_count,
            storyline,
            aggregated_rating,
            aggregated_rating_count,
            status,
            game_status,
            hypes,
            version_parent,
            version_title,
            franchise

        from source

    )

select *
from renamed
