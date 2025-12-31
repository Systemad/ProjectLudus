with
    source as (
        select * from {{ source("igdb_source_20251231072127", "game_localizations") }}
    ),

    renamed as (

        select
            cover__id as id,
            id as game_id,
            cover__alpha_channel as alpha_channel,
            cover__animated as animated,
            cover__height as height,
            cover__image_id as image_id,
            cover__url as url,
            cover__width as width,
            cover__checksum as checksum

        from source

    )

select *
from renamed
where id is not null
