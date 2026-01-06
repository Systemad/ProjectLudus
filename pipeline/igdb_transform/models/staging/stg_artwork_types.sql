with
    source as (select * from {{ source("igdb_source2", "artwork_types") }}),

    renamed as (

        select id, created_at, updated_at, slug, name, checksum, _dlt_load_id, _dlt_id

        from source

    )

select *
from renamed
