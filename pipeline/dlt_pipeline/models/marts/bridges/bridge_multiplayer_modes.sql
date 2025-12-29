{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('dim_games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_id FOREIGN KEY (platform) REFERENCES {{ ref('dim_platforms') }} (id)",
        ],
    )
}}
select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251229083704", "games__multiplayer_modes"),
            except=["_dlt_parent_id", "_dlt_list_idx", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251229083704", "games__multiplayer_modes") }}
