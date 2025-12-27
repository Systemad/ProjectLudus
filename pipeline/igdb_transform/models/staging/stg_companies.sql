with
    source as (select * from {{ source("igdb_raw_v2", "companies") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            change_date,
            change_date_category,
            country,
            description,
            logo__id,
            logo__alpha_channel,
            logo__animated,
            logo__height,
            logo__image_id,
            logo__url,
            logo__width,
            logo__checksum,
            name,
            slug,
            start_date,
            url,
            checksum,
            status,
            start_date_format,
            _dlt_load_id,
            _dlt_id,
            parent,
            changed_company_id

        from source

    )

select *
from renamed
