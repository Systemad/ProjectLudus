with
source as (select * from {{ source("igdb_ref", "regions") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        category,
        identifier,
        checksum

    from source

)

select * from renamed
