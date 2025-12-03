-- models/marts/dim_companies.sql (Your Final Dimension Table)
{{ config(
    materialized='table',
    post_hook=[
        -- NOTE: Ensure there is a comma separating the constraint strings!
        "ALTER TABLE {{ this }} ADD CONSTRAINT pk_companies PRIMARY KEY (id)", 
        
        -- Self-Referencing Constraints (These assume the IDs point to the same table's PK: company_id)
        "ALTER TABLE {{ this }} ADD CONSTRAINT fk_changed_company FOREIGN KEY (changed_company_id) REFERENCES {{ this }} (company_id)",
        "ALTER TABLE {{ this }} ADD CONSTRAINT fk_parent_company FOREIGN KEY (parent_company_id) REFERENCES {{ this }} (company_id)",
        
        -- Logo Constraint (Must reference the Primary Key of your dim_logos table)
        "ALTER TABLE {{ this }} ADD CONSTRAINT fk_company_logo FOREIGN KEY (logo_id) REFERENCES {{ ref('dim_logos') }} (id)" 
        -- NOTE: You must have a dim_logos table created before this runs!
    ]
) }}

SELECT
    -- Select all clean columns from the Staging model
    id,
    logo_id,
    parent_company_id,
    changed_company_id,
    name,
    slug,
    url,
    country,
    description,
    status,
    checksum,
    created_at,
    updated_at,
    change_date,
    change_date_category,
    start_date,
    start_date_format
    
    -- Exclude the array columns (developed, published, websites)
    -- They belong in separate Fact/Junction tables!

FROM
    {{ ref('stg_igdb_raw__companies') }}