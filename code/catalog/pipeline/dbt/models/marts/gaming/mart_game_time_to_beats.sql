{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_game_time_to_beats") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        game_id,
        hastily,
        normally,
        completely,
        count,
        checksum

    from source

)

select *
from renamed
where game_id in (select id from {{ ref("mart_games") }})
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}

