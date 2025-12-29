with
    source as (
        select * from {{ source("igdb_source_20251229083704", "artwork_types") }}
    ),

    renamed as (select id, created_at, updated_at, slug, name, checksum from source)

select *
from renamed
