{{ config(materialized='table') }}

select 
    id,
    name,
    slug,
    start_date,
    url,
    status_name,
    category,
    country,
    description,
    logo__image_id as logo_id,
    logo__url as logo_url
from {{ source('igdb_source', 'companies') }}
