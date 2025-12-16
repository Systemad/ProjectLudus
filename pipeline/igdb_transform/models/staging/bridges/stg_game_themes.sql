{{ config(materialized="view") }}

select g.id as game_id, t.value as theme_id

from {{ source("igdb_raw_v2", "games__themes") }} t

inner join {{ source("igdb_raw_v2", "games") }} g on t._dlt_parent_id = g._dlt_id
