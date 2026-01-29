with
    source as (select * from {{ source("igdb_source", "companies") }}),

    renamed as (

        select
            logo__id as id,
            id as company_id,
            logo__alpha_channel as alpha_channel,
            logo__animated as animated,
            logo__height as height,
            logo__image_id as image_id,
            logo__url as url,
            logo__width as width,
            logo__checksum as checksum

        from source

    )

select *
from renamed
