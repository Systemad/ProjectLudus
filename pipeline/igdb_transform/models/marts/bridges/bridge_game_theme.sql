{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, theme_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('dim_games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_theme_id FOREIGN KEY (theme_id) REFERENCES {{ ref('dim_themes') }} (id)",
        ],
    )
}}

select game_id, theme_id
from {{ ref("stg_game_themes") }}
