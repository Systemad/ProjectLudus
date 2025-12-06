{{ config(materialized="view") }}

with source as (select * from {{ source("igdb_raw", "companies") }}),

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
