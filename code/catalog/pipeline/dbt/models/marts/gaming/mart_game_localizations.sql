with

{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

formatted as (

    select
        l.id,
        l.name,
        l.game,
        l.region,
        l.created_at,
        l.updated_at,
        l.checksum,
        l.cover

    from {{ ref("int_game_localizations") }} l
    inner join {{ ref("mart_games") }} g on l.game = g.id

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

