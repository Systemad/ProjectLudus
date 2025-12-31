{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('dim_games') }} (id)",
        ],
    )
}}
select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251231072127", "games__alternative_names"),
            except=["_dlt_parent_id", "_dlt_list_idx", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251231072127", "games__alternative_names") }}
