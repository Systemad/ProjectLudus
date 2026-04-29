with
    source as (select * from {{ ref("int_games") }}),

    game_metrics as (
        select
            game_id,
            view_count,
            last_24h_visits
        from {{ ref("mart_game_metrics") }}
    ),

    renamed as (

        select
            s.id,
            s.created_at,
            s.updated_at,
            s.cover,
            s.name,
            s.parent_game,
            s.slug,
            s.summary,
            s.url,
            s.checksum,
            s.game_type,
            s.first_release_date as first_release_date_epoch,
            to_timestamp(s.first_release_date::numeric) as first_release_date_utc,
            s.rating,
            s.rating_count,
            s.total_rating,
            s.total_rating_count,
            s.storyline,
            s.aggregated_rating,
            s.aggregated_rating_count,
            s.game_status,
            s.hypes,
            s.version_parent,
            s.version_title,
            s.franchise,
            coalesce(gm.view_count, 0)::bigint as view_count,
            coalesce(gm.last_24h_visits, 0)::bigint as last_24h_visits

        from source s
        left join game_metrics gm on s.id = gm.game_id

    )

select *
from renamed
