{{
    config(
        materialized="table",
        post_hook=["ALTER TABLE {{ this }} ADD PRIMARY KEY (id)"],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_v2", "regions"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_v2", "regions") }}
