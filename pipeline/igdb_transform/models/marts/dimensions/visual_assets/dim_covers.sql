{{ config(materialized="table") }}

select id, alpha_channel, animated, game_id, height, image_id, url, width, checksum
from {{ ref("stg_covers") }}
