{{ config(materialized='table') }}

select 
    id,
    name,
    slug,
    url,
    alternative_name,
    generation,
    platform_type,
    summary,
    platform_logo__alpha_channel as platformAlpha,
    platform_logo__image_id as logo_id,
    platform_logo__url as logo_url
from {{ source('igdb_source', 'langs') }}
