with
    source as (
        select * from {{ source("igdb_source_20251229083704", "companies__websites") }}
    ),

    renamed as (select id, trusted, url, checksum, type from source)

select *
from renamed
