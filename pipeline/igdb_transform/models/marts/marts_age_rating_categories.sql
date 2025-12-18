with
    source as (select * from {{ ref("stg_age_rating_categories") }}),

    renamed as (

        select id, created_at, updated_at, rating, organization, checksum from source

    )

select *
from renamed
