{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, age_rating_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_age_rating_id FOREIGN KEY (age_rating_id) REFERENCES {{ ref('age_ratings') }} (id)",
        ],
    )
}}

select game_id, age_rating_id

from {{ ref("stg_game_age_rating") }}
