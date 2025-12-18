with
    source as (select * from {{ ref("stg_games__videos") }}),

    renamed as (

        select id, game, name, video_id, checksum, _dlt_parent_id, _dlt_list_idx

        from source

    )

select *
from renamed
