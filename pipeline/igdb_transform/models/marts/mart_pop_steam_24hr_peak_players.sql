with
    source as (select * from {{ ref("stg_pop_steam_24hr_peak_players") }}),
    renamed as (
        select
            id,
            game_id,
            popularity_type,
            popularity_source,
            value,
            calculated_at,
            created_at,
            updated_at,
            external_popularity_source
        from source
    )
select *
from renamed
