{{ config(
    materialized="incremental",
    unique_key="game_id",
    on_schema_change="append_new_columns"
) }}

select s.*
from {{ ref("stg_steam__store_details") }} s
inner join {{ ref("mart_games") }} g on s.game_id = g.id
