{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_collection_membership_types") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        description,
        allowed_collection_type,
        checksum

    from source

)

select *
from renamed
where name is not null and checksum is not null and created_at is not null and updated_at is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}


