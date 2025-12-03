{{ config(materialized='table') }}

select 
    id,
    status
from {{ source('igdb_source', 'game_statuses') }}
