with
    source as (select * from {{ source("igdb_source", "games__age_ratings") }}),

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
