{{ config(materialized="view") }}

with
    source as (select * from {{ source("igdb_raw_v2", "companies__websites") }}),
    renamed as (select id, trusted, url, checksum, type from source)
select *
from renamed
