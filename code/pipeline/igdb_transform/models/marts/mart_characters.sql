with
    source as (select * from {{ ref("stg_characters") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            games,
            name,
            slug,
            url,
            checksum,
            mug_shot,
            character_gender,
            character_species,
            description,
            akas

        from source

    )

select *
from renamed
