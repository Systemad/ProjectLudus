{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, game_mode_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('dim_games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_mode_id FOREIGN KEY (game_mode_id) REFERENCES {{ ref('dim_game_modes') }} (id)",
        ],
    )
}}

select game_id, game_mode_id
from {{ ref("stg_game_themes") }}
