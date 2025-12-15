{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (game_engine_id, company_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_game_engine_id FOREIGN KEY (game_engine_id) REFERENCES {{ ref('game_engines') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_company_id FOREIGN KEY (company_id) REFERENCES {{ ref('companies') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=ref("stg_game_engine_company"),
            quote_identifiers=False,
        )
    }}
from {{ ref("stg_game_engine_company") }}
