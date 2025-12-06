{{ config(materialized="table") }}

with
    source as (select * from {{ source("igdb_raw", "age_rating_organizations") }}),
    renamed as (select id, created_at, updated_at, name, checksum from source)

select *
from renamed
