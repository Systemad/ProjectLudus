with
source as (select * from {{ source("igdb_ref", "date_formats") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        format,
        checksum

    from source

)

select *
from renamed
