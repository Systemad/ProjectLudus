with
source as (select * from {{ source("igdb_ref", "game_statuses") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        status,
        checksum

    from source

)

select * from renamed
