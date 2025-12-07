{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_id FOREIGN KEY (platform) REFERENCES {{ ref('platforms') }} (id)",
        ],
    )
}}
select
    {{
        dbt_utils.star(
            from=source("igdb_raw", "multiplayer_modes"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw", "multiplayer_modes") }}
