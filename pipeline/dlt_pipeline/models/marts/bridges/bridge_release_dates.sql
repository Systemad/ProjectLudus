{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('dim_games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_id FOREIGN KEY (platform) REFERENCES {{ ref('dim_platforms') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT release_region_id FOREIGN KEY (release_region) REFERENCES {{ ref('dim_release_date_regions') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT status_id FOREIGN KEY (status) REFERENCES {{ ref('dim_release_date_statuses') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT date_format_id FOREIGN KEY (date_format) REFERENCES {{ ref('dim_date_formats') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251231072127", "games__release_dates"),
            except=["_dlt_parent_id", "_dlt_list_idx", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251231072127", "games__release_dates") }}
