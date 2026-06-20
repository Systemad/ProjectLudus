with
source as (select * from {{ source("igdb_ref", "platform_types") }}),

renamed as (

    select
        id,
        name,
        created_at,
        updated_at,
        checksum

    from source

)

select * from renamed
