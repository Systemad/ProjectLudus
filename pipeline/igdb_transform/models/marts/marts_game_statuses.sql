with
    source as (select * from {{ ref("stg_game_statuses") }}),

    renamed as (select id, created_at, updated_at, status, checksum from source)

select *
from renamed
