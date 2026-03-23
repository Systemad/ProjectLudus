with
    source as (select * from {{ ref("stg_collection_membership_types") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            description,
            allowed_collection_type,
            checksum

        from source

    )

select *
from renamed
