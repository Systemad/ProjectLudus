with
    source as (

        select *
        from {{ source("igdb_source_20251229083704", "game_version_features") }}

    ),

    renamed as (

        select id, created_at, updated_at, category, position, title,
        values, checksum, _dlt_load_id, _dlt_id, description

        from source

    )

select *
from renamed
