with
    source as (select * from {{ source("igdb_raw_v2", "games__age_ratings") }}),

    renamed as (

        select
            id,
            checksum,
            organization,
            rating_category,
            rating_content_descriptions,
            synopsis

        from source

    )

select *
from renamed
