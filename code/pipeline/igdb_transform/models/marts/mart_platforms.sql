with
    source as (select * from {{ ref("stg_platforms") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            alternative_name,
            generation,
            name,
            platform_logo__id as platform_logo,
            slug,
            url,
            checksum,
            platform_type__id as platform_type,
            platform_family__id as platform_family,
            abbreviation,
            summary

        from source

    )

select *
from renamed
