with
source as (select * from {{ source("igdb_ref", "popularity_types") }}),

renamed as (

    select
        id,
        name,
        created_at,
        updated_at

    from source

)

select *
from renamed
