{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, artwork_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_artwork_id FOREIGN KEY (artwork_id) REFERENCES {{ ref('artworks') }} (id)",
        ],
    )
}}

select game_id, artwork_id

from {{ ref("stg_game_artwork") }}
