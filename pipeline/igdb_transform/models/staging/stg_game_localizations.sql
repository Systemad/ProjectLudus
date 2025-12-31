with
    source as (
        select *
        from {{ source("igdb_source_20251231072127", "games__game_localizations") }}
    ),

    renamed as (

        select id, name, game, region, created_at, updated_at, checksum, cover

        from source

    )

select *
from renamed
