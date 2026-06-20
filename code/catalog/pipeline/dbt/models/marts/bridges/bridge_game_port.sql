{{ config(materialized="table") }}

select *
from {{ ref('int_game__port') }}
