{{ config(materialized='table') }}

select 
    id,
    type
from {{ source('igdb_source', 'game_types') }}
