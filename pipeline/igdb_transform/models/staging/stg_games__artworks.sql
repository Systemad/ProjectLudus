with
    source as (

        select * from {{ source("igdb_source_20251231072127", "games__artworks") }}

    ),

    renamed as (

        select
            id,
            alpha_channel,
            animated,
            game,
            height,
            image_id,
            url,
            width,
            checksum,
            artwork_type,
            _dlt_parent_id,
            _dlt_list_idx,
            _dlt_id

        from source

    )

select *
from renamed
