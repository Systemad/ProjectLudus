with
source as (select * from {{ source("steam_raw", "popularity_scores") }}),

renamed as (

    select
        game_id,
        popularity_type,
        value,
        calculated_at,
        fetched_at

    from source

)

select *
from renamed
