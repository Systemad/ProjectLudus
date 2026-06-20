{{ config(
    materialized='incremental',
    unique_key='game_id',
    on_schema_change='append_new_columns',
) }}

with windowed_pricing as (
    select
        game_id,
        steam_app_id,
        initial_cents,
        final_cents,
        discount_percent,
        currency,
        initial_formatted,
        final_formatted,
        captured_at,
        MAX(final_cents) over (
            partition by game_id
            order by captured_at
            range between interval '30 days' preceding and current row
        ) as high_30d,
        MIN(final_cents) over (
            partition by game_id
            order by captured_at
            range between interval '30 days' preceding and current row
        ) as low_30d
    from {{ source('steam_raw', 'store_pricing') }}
    {% if is_incremental() %}
        where
            captured_at > (select MAX(captured_at) from {{ this }})
            or captured_at >= CURRENT_DATE - INTERVAL '30 days'
    {% endif %}
)

select distinct on (game_id) *
from windowed_pricing
order by game_id asc, captured_at desc
