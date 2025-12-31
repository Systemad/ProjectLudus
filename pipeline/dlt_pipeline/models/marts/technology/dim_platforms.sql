{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_family_id FOREIGN KEY (platform_family) REFERENCES {{ ref('dim_platform_families') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_type_id FOREIGN KEY (platform_type) REFERENCES {{ ref('dim_platform_types') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_logo_id FOREIGN KEY (platform_logo) REFERENCES {{ ref('dim_platform_logos') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251231072127", "platforms"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251231072127", "platforms") }}
