with
source as (select * from {{ source("igdb_source", "collection_relation_types") }}),

renamed as (

    select
        id,
        name,
        description,
        allowed_child_type,
        allowed_parent_type,
        created_at,
        updated_at,
        checksum

    from source

)

select * from renamed
