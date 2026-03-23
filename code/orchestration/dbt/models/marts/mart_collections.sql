with
    source as (select * from {{ ref("stg_collections") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            games,
            name,
            slug,
            url,
            checksum,
            type,
            as_child_relations,
            as_parent_relations

        from source

    )

select *
from renamed
