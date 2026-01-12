with
    source as (select * from {{ source("igdb_source", "languages") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            native_name,
            locale,
            checksum,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
