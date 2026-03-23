with
    source as (select * from {{ ref("stg_age_rating_content_description_types") }}),

    renamed as (select id, created_at, updated_at, slug, name, checksum from source)

select *
from renamed
