{{ config(materialized='table') }}

SELECT
  c.id AS company_id,                -- From companies table (_dlt_id as id)
  cd.value AS game_id                -- From companies__developed table value
FROM {{ ref('companies') }} c
JOIN {{ ref('companies__developed') }} cd ON cd._dlt_parent_id = c._dlt_id