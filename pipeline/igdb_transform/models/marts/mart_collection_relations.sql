with
    source as (select * from {{ ref("stg_collection_relations") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            child_collection,
            parent_collection,
            type,
            checksum

        from source

    )

select *
from renamed
