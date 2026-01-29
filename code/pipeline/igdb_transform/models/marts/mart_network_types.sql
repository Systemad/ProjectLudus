with
    source as (select * from {{ ref("stg_network_types") }}),

    renamed as (

        select id, created_at, updated_at, name, event_networks, checksum from source

    )

select *
from renamed
