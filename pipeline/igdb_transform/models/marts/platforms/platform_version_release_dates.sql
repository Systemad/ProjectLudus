{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_version_id FOREIGN KEY (platform_version) REFERENCES {{ ref('platform_versions') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT date_format_id FOREIGN KEY (date_format) REFERENCES {{ ref('date_formats') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT release_region_id FOREIGN KEY (release_region) REFERENCES {{ ref('release_date_regions') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_v2", "platform_version_release_dates"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_v2", "platform_version_release_dates") }}
