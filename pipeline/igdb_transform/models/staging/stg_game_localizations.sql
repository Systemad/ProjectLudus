with
    source as (
        select *
        from {{ source("igdb_source_20251229083704", "games__game_localizations") }}
    ),

    renamed as (

        select id, name, game, region, created_at, updated_at, checksum, cover

        from source

    )

select *
from renamed
