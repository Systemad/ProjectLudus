with
    source as (select * from {{ source("igdb_source2", "platforms") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            alternative_name,
            generation,
            name,
            platform_logo__id,
            platform_logo__alpha_channel,
            platform_logo__animated,
            platform_logo__height,
            platform_logo__image_id,
            platform_logo__url,
            platform_logo__width,
            platform_logo__checksum,
            slug,
            url,
            checksum,
            platform_type__id,
            platform_type__name,
            platform_type__created_at,
            platform_type__updated_at,
            platform_type__checksum,
            _dlt_load_id,
            _dlt_id,
            platform_family__id,
            platform_family__name,
            platform_family__slug,
            platform_family__checksum,
            abbreviation,
            summary

        from source

    )

select *
from renamed
