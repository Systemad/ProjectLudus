with
    source as (select * from {{ ref("stg_game_engines") }}),

    renamed as (

        select id, created_at, updated_at, name, slug, url, checksum, description, logo

        from source

    )

select *
from renamed
