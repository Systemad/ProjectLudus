with
    source as (select * from {{ ref("stg_popularity_types") }}),
    renamed as (
        select
            id,
            name,
            created_at,
            updated_at,
            checksum,
            external_popularity_source

        from source
    )
select *
from renamed
