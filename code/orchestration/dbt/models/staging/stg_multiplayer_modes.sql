with
    source as (select * from {{ source("igdb_source", "games__multiplayer_modes") }}),

    renamed as (

        select
            id,
            campaigncoop,
            dropin,
            game,
            lancoop,
            offlinecoop,
            onlinecoop,
            platform,
            splitscreen,
            checksum,
            offlinemax,
            onlinemax,
            onlinecoopmax,
            offlinecoopmax

        from source

    )

select *
from renamed
