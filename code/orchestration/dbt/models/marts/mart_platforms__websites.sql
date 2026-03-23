with
    source as (select * from {{ ref("stg_platforms__websites") }}),

    renamed as (select value from source)

select *
from renamed
