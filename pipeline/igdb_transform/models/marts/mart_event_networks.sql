with
    source as (select * from {{ ref("int_event_network") }}),

    renamed as (

        select id, created_at, updated_at, event, url, network_type, checksum

        from source

    )

select *
from renamed
