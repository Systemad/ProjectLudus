with
    source as (select * from {{ source("igdb_source", "games__language_supports") }}),

    renamed as (

        select
            id, game, language, language_support_type, created_at, updated_at, checksum
        from source

    )

select *
from renamed
