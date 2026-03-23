with
    source as (select * from {{ ref("stg_release_date_regions") }}),

    renamed as (select id, created_at, updated_at, region, checksum from source)

select *
from renamed
