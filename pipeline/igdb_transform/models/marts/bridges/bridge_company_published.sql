select company_id, game_id from {{ ref("int_company_published") }}
