with
    formatted as (

        select
            id,
            platform_id,
            alpha_channel,
            animated,
            height,
            image_id,
            url,
            width,
            checksum
        from {{ ref("stg_platform_logo") }}
        where id is not null
    )

select *
from formatted
