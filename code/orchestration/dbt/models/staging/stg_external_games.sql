with
    source as (select * from {{ source("igdb_source", "games__external_games") }}),

    renamed as (

        select
            id,
            created_at,
            game,
            name,
            uid,
            updated_at,
            url,
            checksum,
            year,
            category,
            media,
            platform,
            countries,
            external_game_source,
            game_release_format

        from source

    )

select *
from renamed
