{{ config(materialized="view") }}

with source as (select * from {{ source("igdb_raw", "games") }})

select
    id as game_id,
    -- One to one
    cover,
    parent_game,
    version_parent,
    franchise,

    -- unnesting
    game_engines,
    game_modes,
    genres,
    keywords,
    platforms,
    player_perspectives,
    release_dates,
    screenshots,
    themes,
    websites,
    alternative_names,
    artworks,
    external_games,
    involved_companies,
    similar_games,
    tags,
    language_supports,
    videos,
    collections,
    age_ratings,
    franchises,
    game_localizations,
    expanded_games,
    multiplayer_modes,
    remasters,
    remakes,
    expansions,
    standalone_expansions,
    dlcs,
    ports,
    forks,

    name,
    slug,
    summary,
    url,
    game_type,
    storyline,
    aggregated_rating,
    aggregated_rating_count,
    hypes,
    total_rating,
    total_rating_count,
    rating,
    rating_count,
    game_status,
    bundles,
    version_title,

    -- metadata
    status,
    checksum,
    first_release_date,
    created_at,
    updated_at
select *
from source
