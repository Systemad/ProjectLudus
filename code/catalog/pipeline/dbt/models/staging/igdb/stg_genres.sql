with
source as (select * from {{ source("igdb_ref", "genres") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        slug,
        url,
        checksum

    from source

)

select * from renamed
