{{ config(materialized="table", cluster_by=["release_year"], unique_key="id") }}

with
    game_themes as (
        select gt.game_id, array_agg(distinct t.name)::text[] as themes
        from {{ ref("bridge_game_theme") }} gt
        join {{ ref("mart_themes") }} t on gt.theme_id = t.id
        group by gt.game_id
    ),
    game_genres as (
        select gg.game_id, array_agg(distinct gen.name)::text[] as genres
        from {{ ref("bridge_game_genre") }} gg
        join {{ ref("mart_genres") }} gen on gg.genre_id = gen.id
        group by gg.game_id
    ),
    game_modes as (
        select gm.game_id, array_agg(distinct m.name)::text[] as modes
        from {{ ref("bridge_game_game_mode") }} gm
        join {{ ref("mart_game_modes") }} m on gm.game_mode_id = m.id
        group by gm.game_id
    )
select
    g.id,
    g.name,
    g.summary,
    g.storyline,
    g.first_release_date,
    g.game_type,
    img.image_id as cover_url,
    coalesce(gt.themes, array[]::text[]) as themes,
    coalesce(gg.genres, array[]::text[]) as genres,
    coalesce(gm.modes, array[]::text[]) as modes,
    extract(year from to_timestamp(g.first_release_date::numeric))::int as release_year

from {{ ref("mart_games") }} g
left join game_themes gt on g.id = gt.game_id
left join game_genres gg on g.id = gg.game_id
left join game_modes gm on g.id = gm.game_id
left join {{ ref("mart_covers") }} img on g.cover = img.id
