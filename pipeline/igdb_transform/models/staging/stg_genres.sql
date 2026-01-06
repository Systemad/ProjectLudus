with
    source as (select * from {{ source("igdb_source2", "genres") }}),

    renamed as (

        select
            id, created_at, updated_at, name, slug, url, checksum, _dlt_load_id, _dlt_id

        from source

    )

select *
from renamed
