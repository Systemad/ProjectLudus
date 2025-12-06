{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD CONSTRAINT pk_companies PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_changed_company FOREIGN KEY (changed_company_id) REFERENCES {{ this }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_parent_company FOREIGN KEY (parent_company_id) REFERENCES {{ this }} (id)",
        ],
    )
}}
select
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

from {{ ref("stg_companies") }}
