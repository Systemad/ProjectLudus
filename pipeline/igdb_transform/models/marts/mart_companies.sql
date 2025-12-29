with
    source as (select * from {{ ref("stg_companies") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            change_date,
            change_date_category,
            country,
            description,
            logo__id as logo,
            name,
            slug,
            start_date,
            url,
            checksum,
            status,
            start_date_format,
            parent,
            nullif(parent, 0)::bigint as parent,
            nullif(changed_company_id, 0)::bigint as changed_company_id

        from source

    )

select *
from renamed
