with
    source as (
        select * from {{ source("igdb_source_20251231072127", "companies__websites") }}
    ),

    renamed as (select id, trusted, url, checksum, type from source)

select *
from renamed
