{{ config(
    materialized='incremental',
    unique_key='game_id',
    on_schema_change='append_new_columns',
) }}

with latest_row as (
    select distinct on (game_id)
        game_id,
        steam_app_id,
        current_players,
        captured_at
    from {{ source('steam_raw', 'concurrent_users') }}
    order by game_id asc, captured_at desc
),

peak_24h as (
    select
        game_id,
        MAX(current_players) as peak_24h
    from {{ source('steam_raw', 'concurrent_users') }}
    where captured_at >= CURRENT_TIMESTAMP - INTERVAL '24 hours'
    group by game_id
)

select
    l.game_id,
    l.steam_app_id,
    l.current_players,
    l.captured_at,
    p.peak_24h,
    COALESCE((
        select ARRAY_AGG(peak_players order by bucket asc)
        from {{ source('steam', 'steam_player_stats_daily') }} as h
        where
            h.game_id = l.game_id
            and bucket >= CURRENT_DATE - INTERVAL '7 days'
    ), array[]::INTEGER []) as sparkline_7d
from latest_row as l
left join peak_24h as p on l.game_id = p.game_id
