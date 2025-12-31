with
    source as (select * from {{ source("igdb_source_20251231072127", "companies") }}),

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
            nullif(parent, 0)::bigint as parent,
            nullif(changed_company_id, 0)::bigint as changed_company_id
        from source

    )

select *
from renamed
