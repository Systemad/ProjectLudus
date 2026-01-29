with
    source as (select * from {{ ref("stg_release_date_statuses") }}),

    renamed as (

        select id, created_at, updated_at, name, description, checksum from source

    )

select *
from renamed
