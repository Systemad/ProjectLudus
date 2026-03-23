with
    source as (select * from {{ ref("stg_languages") }}),

    renamed as (

        select id, created_at, updated_at, name, native_name, locale, checksum

        from source

    )

select *
from renamed
