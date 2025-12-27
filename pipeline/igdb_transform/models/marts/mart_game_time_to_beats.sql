with
    source as (select * from {{ ref("stg_game_time_to_beats") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            game_id,
            hastily,
            normally,
            completely,
            count,
            checksum

        from source

    )

select *
from renamed
