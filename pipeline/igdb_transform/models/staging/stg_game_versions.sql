with
    source as (select * from {{ source("igdb_source2", "game_versions") }}),

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
