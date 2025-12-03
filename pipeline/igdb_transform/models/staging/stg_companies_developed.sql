-- models/staging/stg_companies_developed.sql
{{ config(materialized='view') }}

SELECT
    t.id AS id,
    -- Use jsonb_array_elements_text to unnest the array of Game IDs
    (jsonb_array_elements_text(t.developed::jsonb))::integer AS game_id
FROM
    {{ source('igdb_raw', 'companies') }} t -- Assuming 'companies' is the raw table name
WHERE
    t.developed IS NOT NULL