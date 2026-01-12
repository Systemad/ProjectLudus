with
    source as (select * from {{ source("igdb_source", "age_rating_categories") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            rating,
            organization,
            checksum,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
