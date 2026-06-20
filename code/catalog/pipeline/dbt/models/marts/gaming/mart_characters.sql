{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_characters") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        slug,
        url,
        checksum,
        mug_shot,
        character_gender,
        character_species,
        description

    from source

)

select *
from renamed
where name is not null and slug is not null and checksum is not null and created_at is not null and updated_at is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}

