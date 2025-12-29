with
    source as (

        select * from {{ source("igdb_source_20251229083704", "game_versions") }}

    ),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            game,
            url,
            checksum,
            _dlt_load_id,
            _dlt_id,
            features,
            games

        from source

    )

select *
from renamed
