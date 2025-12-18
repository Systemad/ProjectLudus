with
    source as (select * from {{ ref("stg_games__release_dates") }}),

    renamed as (

        select
            id,
            created_at,
            date,
            game,
            human,
            m,
            platform,
            updated_at,
            y,
            checksum,
            status,
            date_format,
            release_region,
            _dlt_parent_id,
            _dlt_list_idx,
            _dlt_id

        from source

    )

select *
from renamed
