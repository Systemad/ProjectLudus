{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT type_id FOREIGN KEY (type) REFERENCES {{ ref('dim_website_types') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('dim_games') }} (id)",
        ],
    )
}}
select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251229083704", "games__websites"),
            except=["_dlt_load_id", "_dlt_id", "_dlt_parent_id", "_dlt_list_idx"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251229083704", "games__websites") }}
