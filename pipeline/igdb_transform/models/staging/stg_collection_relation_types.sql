with
    source as (select * from {{ source("igdb_source", "collection_relation_types") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            description,
            allowed_child_type,
            allowed_parent_type,
            checksum,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
