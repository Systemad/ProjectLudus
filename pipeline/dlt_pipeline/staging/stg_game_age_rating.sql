{{ config(materialized="view") }}

select
    t.id as game_id,
    (jsonb_array_elements_text(t.age_ratings::jsonb))::integer as age_rating_id
from {{ source("igdb_raw", "games") }} t
where t.age_ratings is not null
