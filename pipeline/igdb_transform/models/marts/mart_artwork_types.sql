with
    source as (select * from {{ source("igdb_source2", "artwork_types") }}),

    renamed as (select id, created_at, updated_at, slug, name, checksum from source)

select *
from renamed
