with
    source as (select * from {{ ref("stg_collection_types") }}),

    renamed as (

        select id, created_at, updated_at, name, description, checksum from source

    )

select *
from renamed
