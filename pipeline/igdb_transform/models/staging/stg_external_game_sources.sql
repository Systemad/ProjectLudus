with
    source as (select * from {{ source("igdb_source2", "external_game_sources") }}),

    renamed as (

        select id, created_at, updated_at, name, checksum, _dlt_load_id, _dlt_id

        from source

    )

select *
from renamed
