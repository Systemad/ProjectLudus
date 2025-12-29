with
    source as (
        select *
        from {{ source("igdb_source_20251229083704", "games__involved_companies") }}
    ),

    renamed as (

        select
            id,
            company,
            created_at,
            developer,
            game,
            porting,
            publisher,
            supporting,
            updated_at,
            checksum

        from source

    )

select *
from renamed
