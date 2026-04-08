{{ config(materialized="view") }}

select
    id,
    name,
    igdb_visits,
    igdb_want_to_play,
    igdb_playing,
    igdb_played,
    steam_24hr_peak_players,
    steam_positive_reviews,
    steam_negative_reviews,
    steam_total_reviews,
    steam_global_top_sellers,
    steam_most_wishlisted_upcoming,
    twitch_24hr_hours_watched
from {{ ref("mart_games_search") }}
where
    igdb_visits is not null
    or steam_24hr_peak_players is not null
    or twitch_24hr_hours_watched is not null
