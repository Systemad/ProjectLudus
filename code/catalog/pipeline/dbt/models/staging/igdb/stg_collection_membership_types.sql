with
source as (select * from {{ source("igdb_source", "collection_membership_types") }}),

renamed as (

    select
        id,
        name,
        description,
        allowed_collection_type,
        created_at,
        updated_at,
        checksum

    from source

)

select * from renamed
