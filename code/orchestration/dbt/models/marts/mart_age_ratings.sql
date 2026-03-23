with
    formatted as (

        select
            id,
            checksum,
            organization,
            rating_category,
            rating_content_descriptions,
            synopsis

        from {{ ref("stg_age_ratings") }}

    )

select *
from formatted
