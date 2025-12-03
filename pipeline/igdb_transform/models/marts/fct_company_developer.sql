-- models/marts/fct_company_developer.sql
{{ config(
    materialized='table',
    post_hook=[
        "ALTER TABLE {{ this }} ADD CONSTRAINT pk_developer_game PRIMARY KEY (company_id, game_id)",

        -- Ensure company_id exists in the dim_companies table
        "ALTER TABLE {{ this }} ADD CONSTRAINT fk_dev_company FOREIGN KEY (company_id) REFERENCES {{ ref('dim_companies') }} (company_id)",
        -- Ensure game_id exists in the dim_games table (Assuming you have this model)
        "ALTER TABLE {{ this }} ADD CONSTRAINT fk_dev_game FOREIGN KEY (game_id) REFERENCES {{ ref('dim_games') }} (game_id)"
    ]
) }}

SELECT
    company_id,
    game_id
FROM
    {{ ref('stg_igdb_raw__company_developer') }}