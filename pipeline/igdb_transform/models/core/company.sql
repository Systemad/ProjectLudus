{{ config(
        materialized='table',
        post_hook=[
                    "ALTER TABLE {{ this }} ADD CONSTRAINT pk_companies PRIMARY KEY (id)",
                    "ALTER TABLE {{ this }} ADD CONSTRAINT fk_changed_company FOREIGN KEY (changed_company_id) REFERENCES {{ this }} (id)",
                    "ALTER TABLE {{ this }} ADD CONSTRAINT fk_parent_company FOREIGN KEY (parent_company_id) REFERENCES {{ this }} (id)"
                ]
            ) }}

with source as (

    select * from {{ source('igdb_raw', 'companies') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        change_date,
        change_date_category,
        country,
        description,
        developed,
        logo as logo_id_fk,
        name,
        published,
        slug,
        start_date,
        url,
        checksum,
        status,
        start_date_format,
        parent_company_id,
        websites,
        changed_company_id

    from source

)

select * from renamed



-- Essentially, look what EF Core done, and do the same, for post_hook
--    CONSTRAINT pk_companies PRIMARY KEY (id),
--    CONSTRAINT fk_companies_companies_changed_company_id FOREIGN KEY (changed_company_id) REFERENCES companies (id) ON DELETE RESTRICT,
--    CONSTRAINT fk_companies_companies_parent_id FOREIGN KEY (parent_id) REFERENCES companies (id) ON DELETE RESTRICT
---
