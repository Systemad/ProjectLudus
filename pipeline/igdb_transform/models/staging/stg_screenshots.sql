with
    source as (select * from {{ source("igdb_source2", "games__screenshots") }}),

    renamed as (

        select id, alpha_channel, animated, game, height, image_id, url, width, checksum

        from source

    )

select *
from renamed
