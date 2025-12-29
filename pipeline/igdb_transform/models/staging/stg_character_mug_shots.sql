with
    source as (

        select * from {{ source("igdb_source_20251229083704", "character_mug_shots") }}

    ),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            height,
            image_id,
            url,
            width,
            checksum,
            _dlt_load_id,
            _dlt_id,
            alpha_channel,
            animated

        from source

    )

select *
from renamed
