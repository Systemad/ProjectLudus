{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_id FOREIGN KEY (platform) REFERENCES {{ ref('platforms') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT external_game_source FOREIGN KEY (external_game_source) REFERENCES {{ ref('external_game_sources') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_release_format_id FOREIGN KEY (game_release_format) REFERENCES {{ ref('game_release_formats') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw", "external_games"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw", "external_games") }}
