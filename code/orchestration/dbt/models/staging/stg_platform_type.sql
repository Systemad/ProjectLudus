with
    source as (select * from {{ source("igdb_source", "platforms") }}),

    renamed as (

        select
            platform_type__id as id,
            id as platform_id,
            platform_type__name as name,
            platform_type__created_at as created_at,
            platform_type__updated_at as updated_at,
            platform_type__checksum as checksum

        from source

    )

select *
from renamed
where id is not null
