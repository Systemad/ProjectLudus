with
    source as (select * from {{ ref("stg_games__ports") }}),

    renamed as (select value from source)

select *
from renamed
