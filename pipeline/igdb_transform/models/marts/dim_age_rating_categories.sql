-- models/marts/dim_age_rating_categories.sql
{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD CONSTRAINT pk_age_rating_categories PRIMARY KEY (category_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_category_org FOREIGN KEY (organization_fk) REFERENCES {{ ref('dim_age_rating_organizations') }} (id)",
        ],
    )
}}

with
    source as (select * from {{ source("igdb_raw", "age_rating_categories") }}),
    final_select as (
        select
            id as category_id,
            organization as organization_fk,
            rating,
            created_at,
            updated_at,
            checksum
        from source
    )
select *
from final_select
