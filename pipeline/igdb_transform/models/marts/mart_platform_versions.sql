with
    source as (select * from {{ ref("stg_platform_versions") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            platform_logo,
            platform_version_release_dates,
            slug,
            summary,
            url,
            checksum,
            companies,
            cpu,
            media,
            memory,
            output,
            resolutions,
            sound,
            connectivity,
            storage,
            graphics,
            os,
            main_manufacturer

        from source

    )

select *
from renamed
