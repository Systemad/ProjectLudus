{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_id, franchise_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_id FOREIGN KEY (game_id) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_franchise_id FOREIGN KEY (franchise_id) REFERENCES {{ ref('franchises') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=ref("stg_game_franchises"),
            quote_identifiers=False,
        )
    }}
from {{ ref("stg_game_franchises") }}
