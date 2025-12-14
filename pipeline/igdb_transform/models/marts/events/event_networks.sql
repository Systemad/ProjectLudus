{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT event_id FOREIGN KEY (event) REFERENCES {{ ref('events') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT network_type_id FOREIGN KEY (network_type) REFERENCES {{ ref('network_types') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_v2", "event_networks"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_v2", "event_networks") }}
