with
    source as (select * from {{ ref("stg_game_version_features") }}),

    renamed as (

        select id, created_at, updated_at, category, position, title,
        values, checksum, description

        from source

    )

select *
from renamed
