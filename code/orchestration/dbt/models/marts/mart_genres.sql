with
    source as (select * from {{ ref("stg_genres") }}),

    renamed as (

        select id, created_at, updated_at, name, slug, url, checksum from source

    )

select *
from renamed
