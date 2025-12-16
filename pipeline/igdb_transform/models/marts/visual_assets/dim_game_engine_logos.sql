{{
    config(
        materialized="table",
        post_hook=["ALTER TABLE {{ this }} ADD PRIMARY KEY (id)"],
    )
}}

with
    final as (
        select {{ dbt_utils.star(from=ref("stg_game_engine_logos")) }}
        from {{ ref("stg_game_engine_logos") }}
    )
select *
from final
