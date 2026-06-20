with
source as (select * from {{ source("igdb_ref", "platform_families") }}),

renamed as (

    select
        id,
        name,
        slug,
        checksum

    from source

)

select * from renamed
