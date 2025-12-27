with
    source as (select * from {{ ref("stg_games__game_localizations") }}),

    renamed as (

        select
            id,
            name,
            game,
            region,
            created_at,
            updated_at,
            checksum,
            _dlt_parent_id,
            _dlt_list_idx,
            _dlt_id,
            cover

        from source

    )

select *
from renamed
