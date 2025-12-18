{{
    config(
        materialized="table",
        post_hook=["ALTER TABLE {{ this }} ADD PRIMARY KEY (id)"],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_v2", "games__artworks"),
            except=["_dlt_load_id", "_dlt_id", "_dlt_parent_id", "_dlt_list_idx"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_v2", "games__artworks") }}
