with
    source as (select * from {{ ref("int_companies") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            change_date,
            change_date_category,
            country,
            description,
            logo,
            name,
            slug,
            start_date,
            url,
            checksum,
            status,
            start_date_format,
            parent_id,
            changed_company_id

        from source

    )

select *
from renamed
