with
    source as (select * from {{ source("igdb_source", "platforms") }}),

    renamed as (

        select
            platform_logo__id as id,
            id as platform_id,
            platform_logo__alpha_channel as alpha_channel,
            platform_logo__animated as animated,
            platform_logo__height as height,
            platform_logo__image_id as image_id,
            platform_logo__url as url,
            platform_logo__width as width,
            platform_logo__checksum as checksum

        from source

    )

select *
from renamed
where id is not null
