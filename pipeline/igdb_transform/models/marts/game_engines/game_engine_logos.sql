{{
    config(
        materialized="table",
        post_hook=["ALTER TABLE {{ this }} ADD PRIMARY KEY (id)"],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_new", "game_engine_logos"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_new", "game_engine_logos") }}
