with
    source as (select * from {{ source("igdb_source", "games__videos") }}),

    renamed as (

        select
            id, game, name, video_id, checksum, _dlt_parent_id, _dlt_list_idx, _dlt_id

        from source

    )

select *
from renamed
