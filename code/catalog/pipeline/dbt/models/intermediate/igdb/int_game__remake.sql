{{ config(materialized="view") }}

select distinct on (remake_id)
    g.id as remake_source_id,
    related.id as remake_id
from {{ ref('stg_games__remakes') }} as t
inner join {{ ref('stg_games') }} as g on t._dlt_parent_id = g._dlt_id
inner join {{ ref('stg_games') }} as related on t.value = related.id
inner join {{ ref('int_games') }} as mg on g.id = mg.id
inner join {{ ref('int_games') }} as mrg on related.id = mrg.id
order by remake_id, g.id
