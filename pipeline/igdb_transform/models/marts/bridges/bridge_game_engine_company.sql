select game_engine_id, company_id from {{ ref("int_game_engine_company") }}
