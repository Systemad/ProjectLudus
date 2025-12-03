{{ config(materialized='table') }}

select 
    id,
    name
from {{ source('igdb_source', 'ext_game_src') }}
