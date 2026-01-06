with
    source as (select * from {{ source("igdb_source2", "release_date_regions") }}),

    renamed as (

        select id, created_at, updated_at, region, checksum, _dlt_load_id, _dlt_id

        from source

    )

select *
from renamed
