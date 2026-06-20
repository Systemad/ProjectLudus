{{ config(materialized='incremental', unique_key='game_id', on_schema_change='append_new_columns') }}

select distinct on (game_id)
    game_id,
    steam_app_id,
    num_reviews,
    review_score,
    review_score_desc,
    total_positive,
    total_negative,
    total_reviews
from {{ source('steam_raw', 'reviews') }}
order by game_id asc, captured_at desc
