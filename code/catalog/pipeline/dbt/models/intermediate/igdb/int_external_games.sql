with
external_games as (select * from {{ ref("stg_external_games") }}),

games as (select id from {{ ref("int_games") }})

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
    ext.platform,
    ext.external_game_source,
    ext.game_release_format
from external_games as ext
inner join games as g on ext.game = g.id
