with
    formatted as (

        select
            id,
            company_id,
            alpha_channel,
            animated,
            height,
            image_id,
            url,
            width,
            checksum

        from {{ ref("stg_company_logos") }}
        where id is not null
    )
select *
from formatted
