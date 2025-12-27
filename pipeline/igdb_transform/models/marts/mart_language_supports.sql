with
    source as (select * from {{ ref("stg_language_supports") }}),

    renamed as (

        select
            id, game, language, language_support_type, created_at, updated_at, checksum

        from source

    )

select *
from renamed
