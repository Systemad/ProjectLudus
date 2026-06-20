{{ config(materialized="view") }}

select
    g.id as expanded_source_id,
    related.id as expanded_game_id
from {{ ref('stg_games__expanded_games') }} as t
inner join {{ ref('stg_games') }} as g on t._dlt_parent_id = g._dlt_id
inner join {{ ref('stg_games') }} as related on t.value = related.id
inner join {{ ref('int_games') }} as mg on g.id = mg.id
inner join {{ ref('int_games') }} as mrg on related.id = mrg.id
where
    g.id is not null
    and related.id is not null
