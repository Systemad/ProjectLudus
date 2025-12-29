{{ config(materialized="view") }}

select g.id as game_id, t.value as game_engine_id

from {{ source("igdb_source_20251229083704", "games__game_engines") }} t

inner join
    {{ source("igdb_source_20251229083704", "games") }} g
    on t._dlt_parent_id = g._dlt_id
