with
source as (select * from {{ source("igdb_source", "games__age_ratings") }}),

renamed as (

    select
        id,
        checksum,
        organization,
        rating_category,
        synopsis

    from source

)

select *
from renamed
