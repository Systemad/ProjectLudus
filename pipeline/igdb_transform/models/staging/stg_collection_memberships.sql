with
    source as (select * from {{ source("igdb_source2", "collection_memberships") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            game,
            collection,
            type,
            checksum,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
