with
source as (select * from {{ source("igdb_source", "characters") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        slug,
        url,
        checksum,
        _dlt_load_id,
        _dlt_id,
        mug_shot,
        character_gender,
        character_species,
        description

    from source

)

select *
from renamed
