with
source as (select * from {{ source("igdb_source", "collection_relations") }}),

renamed as (

    select
        id,
        child_collection,
        parent_collection,
        type,
        created_at,
        updated_at,
        checksum

    from source

)

select * from renamed
