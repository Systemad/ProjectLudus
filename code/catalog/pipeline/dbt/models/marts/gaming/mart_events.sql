{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_events") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        slug,
        event_logo,
        start_time as start_time_epoch,
        time_zone,
        live_stream_url,
        checksum,
        end_time as end_time_epoch,
        description,
        to_timestamp(start_time::numeric) as start_time_utc,
        to_timestamp(end_time::numeric) as end_time_utc

    from source

)

select *
from renamed
where name is not null and slug is not null and checksum is not null and created_at is not null and updated_at is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}

