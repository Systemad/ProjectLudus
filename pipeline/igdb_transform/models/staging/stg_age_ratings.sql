with
    source as (
        select * from {{ source("igdb_source_20251231072127", "games__age_ratings") }}
    ),

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
