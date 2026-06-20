{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_event_logos") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        event,
        alpha_channel,
        animated,
        height,
        image_id,
        url,
        width,
        checksum

    from source

)

select *
from renamed
{% if is_incremental() %}
where updated_at > (select max(updated_at) from {{ this }})
{% endif %}


