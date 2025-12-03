    -- models/staging/stg_igdb_raw__company_development.sql
{{ config(materialized='view') }}

SELECT
    t.id AS company_id,
    (jsonb_array_elements_text(t.developed::jsonb))::integer AS game_id
FROM
    {{ source('igdb_raw', 'companies') }} t
WHERE
    t.developed IS NOT NULL