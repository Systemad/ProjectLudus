{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
formatted as (

    select
        id,
        created_at,
        game,
        name,
        uid,
        updated_at,
        url,
        checksum,
        year,
        platform,
        external_game_source,
        game_release_format

    from {{ ref("int_external_games") }}

),

deduplicated as (
        {{
            dbt_utils.deduplicate(
                relation="formatted",
                partition_by="id",
                order_by="updated_at desc, created_at desc",
            )
        }}

)

select *
from deduplicated
where name is not null and checksum is not null and created_at is not null and updated_at is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}

