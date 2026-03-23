with
    external_games as (select * from {{ ref("stg_games__external_games") }}),

    games as (select id from {{ ref("mart_games") }})

select
    ext.id,
    ext.created_at,
    ext.game,
    ext.name,
    ext.uid,
    ext.updated_at,
    ext.url,
    ext.checksum,
    ext.year,
    ext.category,
    ext.media,
    ext.platform,
    ext.countries,
    ext.external_game_source,
    ext.game_release_format
from external_games ext
inner join games g on ext.game = g.id
