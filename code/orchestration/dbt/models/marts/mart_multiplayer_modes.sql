with

    formatted as (

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

        from {{ ref("stg_multiplayer_modes") }}

    )

select *
from formatted
where game is not null
    and platform is not null
