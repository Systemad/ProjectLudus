with
source as (select * from {{ source("igdb_source", "companies__company_type_histories") }}),

renamed as (

    select
        _dlt_parent_id,
        value as company_type_id

    from source

)

select *
from renamed
