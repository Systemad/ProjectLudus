{{ config(materialized="table") }}

select s.*
from {{ ref('int_game__similar_game') }} s
inner join {{ ref('mart_games') }} mg on s.similar_source_id = mg.id
inner join {{ ref('mart_games') }} msg on s.similar_game_id = msg.id
