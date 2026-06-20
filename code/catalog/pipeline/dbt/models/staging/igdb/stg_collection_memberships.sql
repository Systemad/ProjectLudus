with
source as (select * from {{ source("igdb_source", "collection_memberships") }}),

renamed as (

    select
        id,
        game,
        collection,
        type,
        created_at,
        updated_at,
        checksum

    from source

)

select * from renamed
