with
source as (select * from {{ source("igdb_ref", "company_statuses") }}),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        checksum

    from source

)

select *
from renamed
