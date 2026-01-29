with
    source as (select * from {{ ref("stg_player_perspectives") }}),

    renamed as (

        select id, created_at, updated_at, name, slug, url, checksum from source

    )

select *
from renamed
