with
    source as (select * from {{ ref("stg_age_rating_content_descriptions_v2") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            description,
            organization,
            checksum,
            description_type

        from source

    )

select *
from renamed
