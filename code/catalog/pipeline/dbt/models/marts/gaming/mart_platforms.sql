{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_platforms") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        alternative_name,
        generation,
        name,
        platform_logo__id as platform_logo,
        slug,
        url,
        checksum,
        platform_type__id as platform_type,
        platform_family__id as platform_family,
        abbreviation,
        summary

    from source

)

select *
from renamed
where name is not null and slug is not null and checksum is not null and created_at is not null and updated_at is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}

