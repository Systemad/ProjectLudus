with
    source as (select * from {{ ref("int_platform_versions") }}),

    renamed as (

        select
            id,
            name,
            platform_logo,
            slug,
            summary,
            url,
            checksum,
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
