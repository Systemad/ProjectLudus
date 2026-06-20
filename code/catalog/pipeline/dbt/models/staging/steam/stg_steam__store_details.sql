{{ config(materialized='incremental', unique_key='game_id', on_schema_change='append_new_columns') }}

select distinct on (game_id)
    game_id,
    steam_app_id,
    header_url,
    capsule_url
from {{ source('steam_raw', 'store_details') }}
order by game_id asc, captured_at desc
