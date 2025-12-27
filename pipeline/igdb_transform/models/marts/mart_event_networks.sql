with
    source as (select * from {{ ref("stg_event_networks") }}),

    renamed as (

        select id, created_at, updated_at, event, url, network_type, checksum

        from source

    )

select *
from renamed
