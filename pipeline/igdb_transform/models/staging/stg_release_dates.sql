with
    source as (
        select * from {{ source("igdb_source_20251229083704", "games__release_dates") }}
    ),

    renamed as (

        select
            id,
            created_at,
            date,
            game,
            human,
            m,
            platform,
            updated_at,
            y,
            checksum,
            status,
            date_format

        from source

    )

select *
from renamed
