{{ config(materialized="view") }}

select
    t.id as game_id,
    (jsonb_array_elements_text(t.artworks::jsonb))::integer as artwork_id
from {{ source("igdb_raw", "games") }} t
where t.artworks is not null
