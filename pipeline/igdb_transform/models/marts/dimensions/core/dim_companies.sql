{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT logo_id FOREIGN KEY (logo) REFERENCES {{ ref('company_logos') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT parent_id FOREIGN KEY (parent) REFERENCES {{ this }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT status_id FOREIGN KEY (status) REFERENCES {{ ref('company_statuses') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT start_date_format_id FOREIGN KEY (start_date_format) REFERENCES {{ ref('date_formats') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT changed_company_id FOREIGN KEY (changed_company) REFERENCES {{ this }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT change_date_format_id FOREIGN KEY (change_date_format) REFERENCES {{ ref('date_formats') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_v2", "companies"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_v2", "companies") }}
