{{ config(materialized="view") }}

select g.id as game_id, t.value as game_mode_id

from {{ source("igdb_raw_v2", "games__game_modes") }} t

inner join {{ source("igdb_raw_v2", "games") }} g on t._dlt_parent_id = g._dlt_id
