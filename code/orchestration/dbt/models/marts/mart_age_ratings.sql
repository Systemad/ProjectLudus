with
    formatted as (

        select
            id,
            checksum,
            organization,
            rating_category,
            coalesce(rating_content_descriptions, '[]'::jsonb) as rating_content_descriptions,
            synopsis

        from {{ ref("stg_age_ratings") }}

    )

select *
from formatted
