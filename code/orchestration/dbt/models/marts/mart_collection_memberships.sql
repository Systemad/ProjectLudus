with
    source as (select * from {{ ref("stg_collection_memberships") }}),

    renamed as (

        select id, created_at, updated_at, game, collection, type, checksum from source

    )

select *
from renamed
