with
    source as (

        select * from {{ source("igdb_source_20251231072127", "game_engines") }}

    ),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            slug,
            url,
            checksum,
            _dlt_load_id,
            _dlt_id,
            description,
            logo__id as logo,
            logo__alpha_channel,
            logo__animated,
            logo__height,
            logo__image_id,
            logo__url,
            logo__width,
            logo__checksum

        from source

    )

select *
from renamed
