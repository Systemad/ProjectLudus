{{ config(materialized="table") }}

select g.id as game_id, t.value as genre_id
from {{ ref("stg_games__genres") }} t
inner join {{ ref("stg_games") }} g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_games") }} mg on g.id = mg.id
inner join {{ ref("mart_genres") }} mgn on t.value = mgn.id
