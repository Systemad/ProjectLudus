{{ config(materialized="incremental", unique_key=["game_id", "date"]) }}

with source as (
    select * from {{ ref("stg_umami_pageviews") }}
    {% if is_incremental() %}
        where _dlt_load_id > (select max(_dlt_load_id) from {{ this }})
    {% endif %}
)

select
    game_id::bigint as game_id,
    current_date as date,
    extract(isoyear from current_date)::int as iso_year,
    extract(week from current_date)::int as iso_week,
    sum(pageviews) as pageviews,
    now() as updated_at
from source
where game_id is not null
group by game_id
