{{ config(
    materialized="incremental",
    unique_key=["game_id", "platform_id"],
    on_schema_change="append_new_columns"
) }}

select distinct
    g.id as game_id,
    t.value as platform_id
from {{ ref("stg_games__platforms") }} as t
inner join {{ ref("stg_games") }} as g on t._dlt_parent_id = g._dlt_id
inner join {{ ref("mart_games") }} as mg on g.id = mg.id
inner join {{ ref("mart_platforms") }} as mp on t.value = mp.id
where t.value is not null
{% if is_incremental() %}
and g.updated_at > (select max(max_games.updated_at) from {{ this }} as b inner join {{ ref("stg_games") }} as max_games on b.game_id = max_games.id)
{% endif %}
