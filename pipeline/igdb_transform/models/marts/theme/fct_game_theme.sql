{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, theme_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_theme_id FOREIGN KEY (theme_id) REFERENCES {{ ref('themes') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=ref("stg_game_themes"),
            quote_identifiers=False,
        )
    }}
from {{ ref("stg_game_themes") }}
