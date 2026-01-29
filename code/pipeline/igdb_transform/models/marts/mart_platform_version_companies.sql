with
    source as (select * from {{ ref("stg_platform_version_companies") }}),

    renamed as (

        select id, company, developer, manufacturer, checksum, comment from source

    )

select *
from renamed
