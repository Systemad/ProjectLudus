with
    source as (select * from {{ ref("stg_date_formats") }}),

    renamed as (select id, created_at, updated_at, format, checksum from source)

select *
from renamed
