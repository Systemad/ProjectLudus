with
    source as (select * from {{ source("igdb_raw_v2", "games__alternative_names") }}),

    renamed as (select id, comment, game, name, checksum from source)

select *
from renamed
