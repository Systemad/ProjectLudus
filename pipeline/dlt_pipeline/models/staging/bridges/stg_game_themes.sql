{{ config(materialized="view") }}

select g.id as game_id, t.value as theme_id

from {{ source("igdb_source_20251231072127", "games__themes") }} t

inner join
    {{ source("igdb_source_20251231072127", "games") }} g
    on t._dlt_parent_id = g._dlt_id
