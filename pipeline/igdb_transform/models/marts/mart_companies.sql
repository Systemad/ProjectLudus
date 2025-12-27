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
            name,
            slug,
            start_date,
            url,
            checksum,
            status,
            start_date_format,
            parent,
            changed_company_id

        from source

    )

select *
from renamed
