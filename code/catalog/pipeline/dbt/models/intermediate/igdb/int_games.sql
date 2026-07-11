with
source as (select * from {{ ref("stg_games") }}),
covers as (select id from {{ ref("mart_covers") }}),
game_types as (select id from {{ ref("mart_game_types") }}),
game_statuses as (select id from {{ ref("mart_game_statuses") }}),

validated as (

    select
        base.id,
        base.created_at,
        base.updated_at,
        case when c.id is not null then base.cover__id end as cover,
        base.name,
        base.parent_game,
        base.slug,
        base.summary,
        base.url,
        base.checksum,
        case when gt.id is not null then base.game_type end as game_type,
        base.first_release_date,
        base.rating,
        base.rating_count,
        base.total_rating,
        base.total_rating_count,
        base.storyline,
        base.aggregated_rating,
        base.aggregated_rating_count,
        case when gs.id is not null then base.game_status end as game_status,
        base.hypes,
        base.version_parent,
        base.version_title,
        base.franchise

    from source as base
    left join covers as c on base.cover__id = c.id
    left join game_types as gt on base.game_type = gt.id
    left join game_statuses as gs on base.game_status = gs.id

)

select *
from validated
where id is not null
