{{ config(materialized="view") }}

select g.id as game_engine_id, t.value as platform_id

from {{ source("igdb_raw_v2", "game_engines__platforms") }} t

inner join {{ source("igdb_raw_v2", "game_engines") }} g on t._dlt_parent_id = g._dlt_id
