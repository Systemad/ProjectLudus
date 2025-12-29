with
    source as (

        select * from {{ source("igdb_source_20251229083704", "platform_versions") }}

    ),

    renamed as (

        select
            id,
            created_at,
            updated_at,
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
