{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, genre_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('dim_games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_genre_id FOREIGN KEY (genre_id) REFERENCES {{ ref('dim_genres') }} (id)",
        ],
    )
}}

select game_id, genre_id
from {{ ref("stg_game_themes") }}
