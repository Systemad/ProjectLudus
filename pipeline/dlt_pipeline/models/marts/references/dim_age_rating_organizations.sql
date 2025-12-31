{{
    config(
        materialized="table",
        post_hook=["ALTER TABLE {{ this }} ADD PRIMARY KEY (id)"],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251231072127", "age_rating_organizations"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251231072127", "age_rating_organizations") }}
