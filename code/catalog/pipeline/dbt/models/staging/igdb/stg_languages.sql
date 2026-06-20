with
source as (select * from {{ source("igdb_ref", "languages") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        native_name,
        locale,
        checksum

    from source

)

select * from renamed
