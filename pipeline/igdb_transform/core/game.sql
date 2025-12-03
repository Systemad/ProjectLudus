{{ config(materialized='table') }}

select 
    id,
    name,
    slug,
    summary,
    storyline,
    url,
    game_type,
    first_release_date,
    aggregated_rating,
    aggregated_rating_count,
    hypes,
    rating,
    rating_count
    total_rating,
    total_rating_count,
    cover__image_id as cover_image_id,
    cover__url as cover_url,
    game_status as game_status_id
from {{ source('igdb_source', 'games') }}
