{{ config(materialized="table") }}

with raw_metrics as (
    select
        game_id,
        session_id,
        first_visited_at,
        last_visited_at
    from {{ source("igdb_source", "game_metrics") }}
)

select
    game_id,
    count(distinct session_id)::bigint as view_count,
    count(distinct case when last_visited_at >= now() - interval '24 hours' then session_id end)::bigint as last_24h_visits,
    min(first_visited_at) as first_visited_at,
    max(last_visited_at) as last_visited_at
from raw_metrics
group by game_id
