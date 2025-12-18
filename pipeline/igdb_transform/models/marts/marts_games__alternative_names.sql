with
    source as (select * from {{ ref("stg_games__alternative_names") }}),

    renamed as (

        select id, comment, game, name, checksum, _dlt_parent_id, _dlt_list_idx

        from source

    )

select *
from renamed
