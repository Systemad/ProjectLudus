{{ config(materialized="table") }}

with
    game_themes as (
        select gt.game_id, array_agg(distinct t.name order by t.name)::text[] as themes
        from {{ ref("bridge_game_theme") }} gt
        join {{ ref("mart_themes") }} t on gt.theme_id = t.id
        group by gt.game_id
    ),
    game_genres as (
        select
            gg.game_id, array_agg(distinct gen.name order by gen.name)::text[] as genres
        from {{ ref("bridge_game_genre") }} gg
        join {{ ref("mart_genres") }} gen on gg.genre_id = gen.id
        group by gg.game_id
    ),
    game_modes as (
        select
            gm.game_id, array_agg(distinct m.name order by m.name)::text[] as game_modes
        from {{ ref("bridge_game_game_mode") }} gm
        join {{ ref("mart_game_modes") }} m on gm.game_mode_id = m.id
        group by gm.game_id
    ),
    game_platforms as (
        select
            gp.game_id, array_agg(distinct p.name order by p.name)::text[] as platforms
        from {{ ref("bridge_game_platform") }} gp
        join {{ ref("mart_platforms") }} p on gp.platform_id = p.id
        group by gp.game_id
    ),
    game_engines as (
        select
            ge.game_id,
            array_agg(distinct e.name order by e.name)::text[] as game_engines
        from {{ ref("bridge_game__game_engine") }} ge
        join {{ ref("mart_game_engines") }} e on ge.game_engine_id = e.id
        group by ge.game_id
    ),
    game_player_perspectives as (
        select
            gpp.game_id,
            array_agg(distinct pp.name order by pp.name)::text[] as player_perspectives
        from {{ ref("bridge_game_player_perspective") }} gpp
        join
            {{ ref("mart_player_perspectives") }} pp
            on gpp.player_perspective_id = pp.id
        group by gpp.game_id
    ),
    game_publishers as (
        select
            ic.game as game_id,
            array_agg(distinct c.name order by c.name)::text[] as publishers
        from {{ ref("mart_involved_companies") }} ic
        join {{ ref("mart_companies") }} c on ic.company = c.id
        where ic.publisher = true
        group by ic.game
    ),
    game_developers as (
        select
            ic.game as game_id,
            array_agg(distinct c.name order by c.name)::text[] as developers
        from {{ ref("mart_involved_companies") }} ic
        join {{ ref("mart_companies") }} c on ic.company = c.id
        where ic.developer = true
        group by ic.game
    ),
    game_multiplayer_flags as (
        select
            gmm.game_id,
            bool_or(mm.onlinecoop) as onlinecoop,
            bool_or(mm.offlinecoop) as offlinecoop,
            bool_or(mm.campaigncoop) as campaigncoop,
            bool_or(mm.lancoop) as lancoop,
            bool_or(mm.splitscreen) as splitscreen,
            bool_or(mm.dropin) as dropin
        from {{ ref("bridge_game_multiplayer_mode") }} gmm
        join {{ ref("mart_multiplayer_modes") }} mm on gmm.multiplayer_mode_id = mm.id
        group by gmm.game_id
    ),
    game_multiplayer_modes as (
        select
            gmf.game_id,
            array_remove(
                array[
                    case when gmf.onlinecoop then 'online_coop' end,
                    case when gmf.offlinecoop then 'offline_coop' end,
                    case when gmf.campaigncoop then 'campaign_coop' end,
                    case when gmf.lancoop then 'lan_coop' end,
                    case when gmf.splitscreen then 'split_screen' end,
                    case when gmf.dropin then 'drop_in' end
                ],
                null
            )::text[] as multiplayer_modes
        from game_multiplayer_flags gmf
    ),
    game_types as (select id, type as game_type from {{ ref("mart_game_types") }}),
    game_statuses as (
        select id, status as game_status from {{ ref("mart_game_statuses") }}
    )
select
    g.id,
    g.name,
    g.summary,
    g.storyline,
    g.first_release_date::bigint as first_release_date,
    gtype.game_type,
    gs.game_status,
    img.image_id as cover_url,
    coalesce(gth.themes, array[]::text[]) as themes,
    coalesce(gg.genres, array[]::text[]) as genres,
    coalesce(gm.game_modes, array[]::text[]) as game_modes,
    coalesce(gp.platforms, array[]::text[]) as platforms,
    coalesce(ge.game_engines, array[]::text[]) as game_engines,
    coalesce(gpp.player_perspectives, array[]::text[]) as player_perspectives,
    coalesce(pub.publishers, array[]::text[]) as publishers,
    coalesce(dev.developers, array[]::text[]) as developers,
    coalesce(gmm.multiplayer_modes, array[]::text[]) as multiplayer_modes,
    case
        when g.first_release_date is not null and g.first_release_date > 0
        then extract(year from to_timestamp(g.first_release_date::numeric))::int
        else null
    end as release_year
from {{ ref("mart_games") }} g
left join game_themes gth on g.id = gth.game_id
left join game_genres gg on g.id = gg.game_id
left join game_modes gm on g.id = gm.game_id
left join game_platforms gp on g.id = gp.game_id
left join game_engines ge on g.id = ge.game_id
left join game_player_perspectives gpp on g.id = gpp.game_id
left join game_publishers pub on g.id = pub.game_id
left join game_developers dev on g.id = dev.game_id
left join game_multiplayer_modes gmm on g.id = gmm.game_id
left join game_types gtype on g.game_type = gtype.id
left join game_statuses gs on g.game_status = gs.id
left join {{ ref("mart_covers") }} img on g.cover = img.id
