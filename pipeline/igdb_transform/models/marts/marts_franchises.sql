with
    source as (select * from {{ ref("stg_franchises") }}),

    renamed as (

        select id, created_at, updated_at, games, name, slug, url, checksum from source

    )

select *
from renamed
