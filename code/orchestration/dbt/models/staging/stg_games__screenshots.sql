with
    source as (select * from {{ source("igdb_source", "games__screenshots") }}),

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
            _dlt_parent_id,
            _dlt_list_idx,
            _dlt_id

        from source

    )

select *
from renamed
