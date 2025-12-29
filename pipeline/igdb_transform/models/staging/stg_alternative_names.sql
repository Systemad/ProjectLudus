with
    source as (
        select *
        from {{ source("igdb_source_20251229083704", "games__alternative_names") }}
    ),

    renamed as (select id, comment, game, name, checksum from source)

select *
from renamed
