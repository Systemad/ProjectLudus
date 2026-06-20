{{ config(
    materialized="incremental",
    unique_key=["game_id", "multiplayer_mode_id"],
    on_schema_change="append_new_columns",
    post_hook=[
        "create index if not exists idx_game_multiplayer_mode_multiplayer_mode_id on {{ this }} (multiplayer_mode_id)",
    ],
) }}

select distinct
    g.id as game_id,
    mm.id as multiplayer_mode_id
from {{ ref("stg_multiplayer_modes") }} as t
inner join {{ ref("stg_games") }} as g on t.game = g.id
inner join {{ ref("int_games") }} as mg on g.id = mg.id
inner join
    {{ ref("mart_multiplayer_modes") }} as mm
    on t.id = mm.id and g.id = mm.game
where mm.id is not null
{% if is_incremental() %}
and g.updated_at > (select max(max_games.updated_at) from {{ this }} as b inner join {{ ref("stg_games") }} as max_games on b.game_id = max_games.id)
{% endif %}
