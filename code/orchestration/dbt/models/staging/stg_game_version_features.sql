with
    source as (select * from {{ source("igdb_source", "game_version_features") }}),

    renamed as (

        select id, category, position, title,
        values, checksum, _dlt_load_id, _dlt_id, description

        from source

    )

select *
from renamed
