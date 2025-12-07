{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT region_id FOREIGN KEY (region) REFERENCES {{ ref('regions') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT cover_id FOREIGN KEY (cover) REFERENCES {{ ref('covers') }} (id)",
        ],
    )
}}
select
    {{
        dbt_utils.star(
            from=source("igdb_raw", "game_localizations"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw", "game_localizations") }}
