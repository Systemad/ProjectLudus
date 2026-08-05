{{ config(
    materialized="incremental",
    unique_key=["game_id", "popularity_type"],
    on_schema_change="append_new_columns"
) }}

select distinct on (s.game_id, s.popularity_type)
    s.game_id,
    s.popularity_type,
    s.value,
    s.calculated_at,
    s.fetched_at as captured_at
from {{ ref("stg_popularity_primitive") }} s
inner join {{ ref("mart_games") }} g on s.game_id = g.id
order by s.game_id, s.popularity_type, s.fetched_at desc
