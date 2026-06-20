with

{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

formatted as (

    select
        id,
        name,
        game,
        region,
        created_at,
        updated_at,
        checksum,
        cover

    from {{ ref("int_game_localizations") }}

)

select *
from formatted
where
    name is not null
    and checksum is not null
    and created_at is not null
    and updated_at is not null
    and game is not null
    and region is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}

