with

    formatted as (

        select id, name, game, region, created_at, updated_at, checksum, cover

        from {{ ref("stg_game_localizations") }}

    )

select *
from formatted
