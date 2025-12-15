{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, genre_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_genre_id FOREIGN KEY (genre_id) REFERENCES {{ ref('genres') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=ref("stg_game_genres"),
            quote_identifiers=False,
        )
    }}
from {{ ref("stg_game_genres") }}
