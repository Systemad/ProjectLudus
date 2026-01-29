with
    source as (select * from {{ ref("stg_external_game_sources") }}),

    renamed as (select id, created_at, updated_at, name, checksum from source)

select *
from renamed
