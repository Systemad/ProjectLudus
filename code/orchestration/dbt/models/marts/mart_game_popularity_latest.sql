with
    source as (select * from {{ ref("mart_popularity_primitives") }}),
    ranked as (
        select
            id,
            game_id,
            popularity_type,
            external_popularity_source,
            value,
            calculated_at,
            created_at,
            updated_at,
            updated_at_tz,
            snapshot_date,
            row_number() over (
                partition by game_id, popularity_type, external_popularity_source
                order by snapshot_date desc, updated_at desc, calculated_at desc, created_at desc, id desc
            ) as rn
        from source
    )
select
    id,
    game_id,
    popularity_type,
    external_popularity_source,
    value,
    calculated_at,
    created_at,
    updated_at,
    updated_at_tz,
    snapshot_date
from ranked
where rn = 1
