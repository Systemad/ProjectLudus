with
    source as (select * from {{ ref("stg_platforms__versions") }}),

    renamed as (select value from source)

select *
from renamed
