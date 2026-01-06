with
    source as (select * from {{ source("igdb_source2", "platform_versions") }}),

    renamed as (

        select
            id,
            name,
            platform_logo,
            slug,
            summary,
            url,
            checksum,
            _dlt_load_id,
            _dlt_id,
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
