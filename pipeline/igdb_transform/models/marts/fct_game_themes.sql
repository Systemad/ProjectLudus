-- models/marts/fct_game_themes.sql
{{
    config(
        materialized="table",
        description="Junction table linking games to their themes.",
        post_hook=[
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_theme_game FOREIGN KEY (game_id) REFERENCES {{ ref('dim_games') }} (game_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_theme_theme FOREIGN KEY (theme_id) REFERENCES {{ ref('dim_themes') }} (id)",
        ],
    )
}}

select game_id, theme_id
from {{ ref("stg_game_themes") }}
