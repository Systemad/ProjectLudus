with
    source as (select * from {{ source("igdb_source2", "games__alternative_names") }}),

    renamed as (select id, comment, game, name, checksum from source)

select *
from renamed
