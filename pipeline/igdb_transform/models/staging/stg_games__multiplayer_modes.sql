with
    source as (

        select *
        from {{ source("igdb_source_20251229083704", "games__multiplayer_modes") }}

    ),

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
            _dlt_parent_id,
            _dlt_list_idx,
            _dlt_id,
            offlinemax,
            onlinemax,
            onlinecoopmax,
            offlinecoopmax

        from source

    )

select *
from renamed
