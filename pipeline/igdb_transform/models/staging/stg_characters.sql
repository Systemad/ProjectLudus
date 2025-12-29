with
    source as (select * from {{ source("igdb_source_20251229083704", "characters") }}),

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
            _dlt_load_id,
            _dlt_id,
            mug_shot,
            character_gender,
            character_species,
            description,
            akas

        from source

    )

select *
from renamed
