with
    source as (
        select *
        from {{ source("igdb_source_20251231072127", "games__alternative_names") }}
    ),

    renamed as (select distinct id, comment, game, name, checksum from source)

select *
from renamed
