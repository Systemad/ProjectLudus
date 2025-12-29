with
    source as (

        select * from {{ source("igdb_source_20251229083704", "game_time_to_beats") }}

    ),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            game_id,
            hastily,
            normally,
            completely,
            count,
            checksum,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
