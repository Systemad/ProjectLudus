{{ config(
    materialized="incremental",
    unique_key=["game_id", "player_perspective_id"],
    on_schema_change="append_new_columns"
) }}

select
    g.id as game_id,
    t.value as player_perspective_id
from {{ ref("stg_games__player_perspectives") }} as t
inner join {{ ref("stg_games") }} as g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_games") }} as mg on g.id = mg.id
inner join {{ ref("mart_player_perspectives") }} as mp on t.value = mp.id
{% if is_incremental() %}
where g.updated_at > (select max(max_games.updated_at) from {{ this }} as b inner join {{ ref("stg_games") }} as max_games on b.game_id = max_games.id)
{% endif %}
