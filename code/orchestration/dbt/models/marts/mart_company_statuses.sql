with
    source as (select * from {{ ref("stg_company_statuses") }}),

    renamed as (select id, created_at, updated_at, name, checksum from source)

select *
from renamed
