with
    source as (select * from {{ ref("stg_platform_version_companies") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            company,
            developer,
            manufacturer,
            checksum,
            comment

        from source

    )

select *
from renamed
