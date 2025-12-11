{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT company_id FOREIGN KEY (company) REFERENCES {{ ref('companies') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('games') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_new", "games__involved_companies"),
            except=["_dlt_parent_id", "_dlt_list_idx", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_new", "games__involved_companies") }}
