with
formatted as (

    select
        id,
        checksum,
        organization,
        rating_category,
        synopsis

    from {{ ref("stg_age_ratings") }}

)

select *
from formatted

