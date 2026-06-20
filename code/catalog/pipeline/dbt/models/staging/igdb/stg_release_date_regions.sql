with
source as (select * from {{ source("igdb_ref", "release_date_regions") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        region,
        checksum

    from source

)

select * from renamed
