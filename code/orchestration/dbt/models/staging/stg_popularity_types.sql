with
    source as (select * from {{ source("igdb_source", "popularity_types") }}),
    renamed as (
        select
            id,
            name,
            created_at,
            updated_at,
            checksum,
            external_popularity_source,
            _dlt_load_id,
            _dlt_id
        from source
    )
select *
from renamed
