with
    source as (select * from {{ ref("stg_game_versions") }}),

    renamed as (

        select id, created_at, updated_at, game, url, checksum, features, games

        from source

    )

select *
from renamed
