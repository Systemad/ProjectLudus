{{ config(materialized="view") }}

select g.id as game_engine_id, t.value as company_id

from {{ source("igdb_source_20251229083704", "game_engines__companies") }} t

inner join
    {{ source("igdb_source_20251229083704", "game_engines") }} g
    on t._dlt_parent_id = g._dlt_id
