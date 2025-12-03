{{ config(materialized='view')}}

with source as (
    select * from {{ source('igdb_raw', 'companies') }}
),


    select
        id,

        -- Foreign keys, one to ne
        logo as logo_id,
        parent_company_id,
        changed_company_id

        -- array for unnesting (many to many)
        developed,
        published,
        websites,

        country,
        description,
        name,
        slug,
        url,
        status,

        -- Metadata
        checksum,
        start_date_format,
        change_date,
        change_date_category,
        start_date,
        created_at,
        updated_at,


from source



-- Essentially, look what EF Core done, and do the same, for post_hook
--    CONSTRAINT pk_companies PRIMARY KEY (id),
--    CONSTRAINT fk_companies_companies_changed_company_id FOREIGN KEY (changed_company_id) REFERENCES companies (id) ON DELETE RESTRICT,
--    CONSTRAINT fk_companies_companies_parent_id FOREIGN KEY (parent_id) REFERENCES companies (id) ON DELETE RESTRICT
---
