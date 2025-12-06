-- models/staging/stg_igdb_raw__game_themes.sql
{{ config(materialized="view") }}

select
    t.id as game_id, (jsonb_array_elements_text(t.themes::jsonb))::integer as theme_id
from {{ source("igdb_raw", "games") }} t
where t.themes is not null
