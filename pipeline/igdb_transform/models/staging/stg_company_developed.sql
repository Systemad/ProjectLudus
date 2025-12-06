-- models/staging/stg_companies_developed.sql
{{ config(materialized="view") }}

select
    t.id as id,
    -- Use jsonb_array_elements_text to unnest the array of Game IDs
    (jsonb_array_elements_text(t.developed::jsonb))::integer as game_id
from {{ source("igdb_raw", "companies") }} t  -- Assuming 'companies' is the raw table name
where t.developed is not null
