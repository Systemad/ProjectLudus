{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_platform_version_release_dates") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        date,
        human,
        m,
        y,
        checksum,
        date_format,
        release_region

    from source

)

select *
from renamed
{% if is_incremental() %}
where updated_at > (select max(updated_at) from {{ this }})
{% endif %}


