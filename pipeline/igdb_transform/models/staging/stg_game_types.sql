with
    source as (select * from {{ source("igdb_source_20251229083704", "game_types") }}),

    renamed as (

        select id, created_at, updated_at, type, checksum, _dlt_load_id, _dlt_id

        from source

    )

select *
from renamed
