{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT event_id FOREIGN KEY (event) REFERENCES {{ ref('dim_events') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT network_type_id FOREIGN KEY (network_type) REFERENCES {{ ref('dim_network_types') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251231072127", "event_networks"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251231072127", "event_networks") }}
