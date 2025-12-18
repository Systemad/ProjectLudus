with
    source as (select * from {{ ref("stg_collection_relation_types") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            description,
            allowed_child_type,
            allowed_parent_type,
            checksum

        from source

    )

select *
from renamed
