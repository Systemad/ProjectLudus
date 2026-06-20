with
source as (select * from {{ source("igdb_ref", "collection_types") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        description,
        checksum

    from source

)

select * from renamed
