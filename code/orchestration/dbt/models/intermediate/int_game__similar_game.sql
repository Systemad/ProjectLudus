{{ config(materialized="view") }}

select
    g.id as game_id,
    similar_game.id as similar_game_id
from {{ ref('stg_games__similar_games') }} t
inner join {{ ref('stg_games') }} g on t._dlt_parent_id = g._dlt_id
inner join {{ ref('stg_games') }} similar_game on t.value = similar_game.id
where g.id is not null
  and similar_game.id is not null
