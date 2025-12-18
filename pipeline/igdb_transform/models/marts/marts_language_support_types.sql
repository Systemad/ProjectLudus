with
    source as (select * from {{ ref("stg_language_support_types") }}),

    renamed as (select id, created_at, updated_at, name, checksum from source)

select *
from renamed
