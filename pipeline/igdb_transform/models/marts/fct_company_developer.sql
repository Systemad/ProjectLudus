-- models/marts/fct_company_developer.sql
{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD CONSTRAINT pk_developer_game PRIMARY KEY (company_id, game_id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_dev_company FOREIGN KEY (company_id) REFERENCES {{ ref('dim_companies') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT fk_dev_game FOREIGN KEY (game_id) REFERENCES {{ ref('dim_games') }} (id)",
        ],
    )
}}

select company_id, game_id
from {{ ref("stg_company_developed") }}
