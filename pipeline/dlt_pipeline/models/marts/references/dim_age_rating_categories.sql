{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT organization_id FOREIGN KEY (organization) REFERENCES {{ ref('dim_age_rating_organizations') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251229083704", "age_rating_categories"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251229083704", "age_rating_categories") }}
