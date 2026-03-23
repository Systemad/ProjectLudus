with
    source as (select * from {{ ref("stg_games__age_ratings") }}),

    renamed as (

        select
            id,
            checksum,
            organization,
            rating_category,
            _dlt_parent_id,
            _dlt_list_idx,
            _dlt_id,
            rating_content_descriptions,
            synopsis

        from source

    )

select *
from renamed
