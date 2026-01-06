with
    source as (select * from {{ source("igdb_source2", "date_formats") }}),

    renamed as (

        select id, created_at, updated_at, format, checksum, _dlt_load_id, _dlt_id

        from source

    )

select *
from renamed
