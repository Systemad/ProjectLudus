{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, game_mode_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_mode_id FOREIGN KEY (game_mode_id) REFERENCES {{ ref('game_modes') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=ref("stg_game_game_mode"),
            quote_identifiers=False,
        )
    }}
from {{ ref("stg_game_game_mode") }}
