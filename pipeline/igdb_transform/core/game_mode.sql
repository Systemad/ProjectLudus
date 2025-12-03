{{ config(materialized='table') }}

select 
    id,
    name,
    slug,
    url
from {{ source('igdb_source', 'game_modes') }}
