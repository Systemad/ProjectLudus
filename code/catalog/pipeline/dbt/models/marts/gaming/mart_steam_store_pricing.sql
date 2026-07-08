{{ config(
    materialized="incremental",
    unique_key="game_id",
    on_schema_change="append_new_columns"
) }}

select * from {{ ref("stg_steam__store_pricing") }}
