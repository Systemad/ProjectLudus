with
    source as (
        select * from {{ source("igdb_source_20251229083704", "games__videos") }}
    ),

    renamed as (select id, game, name, video_id, checksum from source)

select *
from renamed
