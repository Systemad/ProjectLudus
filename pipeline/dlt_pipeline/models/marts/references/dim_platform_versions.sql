{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_logo_id FOREIGN KEY (platform_logo) REFERENCES {{ ref('dim_platform_logos') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT main_manufacturer_id FOREIGN KEY (main_manufacturer) REFERENCES {{ ref('dim_platform_version_companies') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251229083704", "platform_versions"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251229083704", "platform_versions") }}
