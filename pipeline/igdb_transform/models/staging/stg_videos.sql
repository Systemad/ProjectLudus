with
    source as (select * from {{ source("igdb_source2", "games__videos") }}),

    renamed as (select id, game, name, video_id, checksum from source)

select *
from renamed
