with
    source as (select * from {{ source("igdb_raw_v2", "games__language_supports") }}),

    renamed as (

        select
            id, game, language, language_support_type, created_at, updated_at, checksum
        from source

    )

select *
from renamed
