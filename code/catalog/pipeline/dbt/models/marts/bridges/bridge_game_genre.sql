{{ config(
    materialized="incremental",
    unique_key=["game_id", "genre_id"],
    on_schema_change="append_new_columns"
) }}

select
    g.id as game_id,
    t.value as genre_id
from {{ ref("stg_games__genres") }} as t
inner join {{ ref("stg_games") }} as g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_games") }} as mg on g.id = mg.id
inner join {{ ref("mart_genres") }} as mgn on t.value = mgn.id
{% if is_incremental() %}
where g.updated_at > (select max(max_games.updated_at) from {{ this }} as b inner join {{ ref("stg_games") }} as max_games on b.game_id = max_games.id)
{% endif %}
