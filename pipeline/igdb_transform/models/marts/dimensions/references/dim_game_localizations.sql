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
            from=source("igdb_raw_v2", "games__game_localizations"),
            except=["_dlt_parent_id", "_dlt_list_idx", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_v2", "games__game_localizations") }}
