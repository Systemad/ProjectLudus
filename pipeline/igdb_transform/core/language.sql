{{ config(materialized='table') }}

select 
    id,
    name,
    native_name,
    locale,
from {{ source('igdb_source', 'langs') }}
