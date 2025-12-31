with
    source as (
        select * from {{ source("igdb_source_20251231072127", "games__websites") }}
    ),

    renamed as (select id, game, trusted, url, checksum, type from source)

select *
from renamed
