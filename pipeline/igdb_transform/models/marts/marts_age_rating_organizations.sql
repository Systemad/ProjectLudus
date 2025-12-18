with
    source as (select * from {{ ref("stg_age_rating_organizations") }}),

    renamed as (select id, created_at, updated_at, name, checksum from source)

select *
from renamed
