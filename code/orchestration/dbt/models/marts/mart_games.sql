with
    source as (select * from {{ ref("int_games") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            cover,
            name,
            parent_game,
            slug,
            summary,
            url,
            checksum,
            game_type,
            first_release_date as first_release_date_epoch,
            to_timestamp(first_release_date::numeric) as first_release_date_utc,
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
