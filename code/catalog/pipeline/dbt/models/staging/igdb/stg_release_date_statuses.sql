with
source as (select * from {{ source("igdb_ref", "release_date_statuses") }}),

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
