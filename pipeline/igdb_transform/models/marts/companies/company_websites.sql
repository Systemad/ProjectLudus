{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT type_id FOREIGN KEY (type) REFERENCES {{ ref('website_types') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=ref("stg_company_websites"),
            quote_identifiers=False,
        )
    }}
from {{ ref("stg_company_websites") }}
