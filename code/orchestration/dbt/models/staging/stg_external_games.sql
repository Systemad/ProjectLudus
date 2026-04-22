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
            platform,
            countries,
            external_game_source,
            game_release_format

        from source

    )

select *
from
    (
        select
            *,
            row_number() over (
                partition by id order by coalesce(updated_at, created_at) desc
            ) as rn
        from renamed
    ) t
where rn = 1
