with
    formatted as (

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

        from {{ ref("int_external_games") }}

    )

select *
from formatted
