{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('dim_games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT platform_id FOREIGN KEY (platform) REFERENCES {{ ref('dim_platforms') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT external_game_source FOREIGN KEY (external_game_source) REFERENCES {{ ref('dim_external_game_sources') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_release_format_id FOREIGN KEY (game_release_format) REFERENCES {{ ref('dim_game_release_formats') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_source_20251231072127", "games__external_games"),
            except=["_dlt_parent_id", "_dlt_list_idx", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_source_20251231072127", "games__external_games") }}
