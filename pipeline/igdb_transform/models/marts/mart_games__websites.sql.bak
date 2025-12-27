with
    source as (select * from {{ ref("stg_games__websites") }}),

    renamed as (

        select
            id,
            game,
            trusted,
            url,
            checksum,
            type,
            _dlt_parent_id,
            _dlt_list_idx,
            _dlt_id

        from source

    )

select *
from renamed
