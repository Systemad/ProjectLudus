with
source as (select * from {{ source("igdb_ref", "game_types") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        type,
        checksum

    from source

)

select * from renamed
