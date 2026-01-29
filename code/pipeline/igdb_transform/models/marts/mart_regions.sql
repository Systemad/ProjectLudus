with
    source as (select * from {{ ref("stg_regions") }}),

    renamed as (

        select id, created_at, updated_at, name, category, identifier, checksum

        from source

    )

select *
from renamed
