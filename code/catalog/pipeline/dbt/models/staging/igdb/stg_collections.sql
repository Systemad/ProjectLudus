with
source as (select * from {{ source("igdb_source", "collections") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        slug,
        url,
        checksum,
        type

    from source

)

select * from renamed
