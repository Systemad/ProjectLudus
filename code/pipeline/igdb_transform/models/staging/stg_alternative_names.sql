with
    source as (select * from {{ source("igdb_source", "games__alternative_names") }}),

    renamed as (select id, comment, game, name, checksum from source)

select *
from renamed
