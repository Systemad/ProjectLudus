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
            game_release_format

        from {{ ref("stg_external_games") }}

    )

select *
from formatted
