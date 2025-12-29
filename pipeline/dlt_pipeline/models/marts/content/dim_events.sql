{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT event_logo_id FOREIGN KEY (event_logo) REFERENCES {{ ref('dim_event_logos') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251229083704", "events"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251229083704", "events") }}
