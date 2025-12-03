{{ config(materialized='table') }}

select 
    id,
    name,
    slug,
    url,
    logo__image_id as logo_image_id,
    logo__url as logo_url
from {{ source('igdb_source', 'game_eng') }}
