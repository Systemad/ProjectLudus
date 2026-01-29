with
    source as (select * from {{ ref("stg_games__collections") }}),

    renamed as (select value from source)

select *
from renamed
