{{ config(
    materialized="incremental",
    unique_key=["game_id", "popularity_type"],
    on_schema_change="append_new_columns"
) }}

select distinct on (game_id, popularity_type)
    game_id,
    popularity_type,
    value,
    calculated_at,
    fetched_at as captured_at
from {{ ref("stg_popularity_primitive") }}
order by game_id, popularity_type, fetched_at desc
