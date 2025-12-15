{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_engine_id, platform_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_engine_id FOREIGN KEY (game_engine_id) REFERENCES {{ ref('game_engines') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_platform_id FOREIGN KEY (platform_id) REFERENCES {{ ref('platforms') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=ref("stg_game_engine_platform"),
            quote_identifiers=False,
        )
    }}
from {{ ref("stg_game_engine_platform") }}
