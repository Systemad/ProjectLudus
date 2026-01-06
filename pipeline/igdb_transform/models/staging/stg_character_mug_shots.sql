with
    source as (select * from {{ source("igdb_source2", "character_mug_shots") }}),

    renamed as (

        select
            id,
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
