with
    source as (select * from {{ ref("stg_games__remakes") }}),

    renamed as (select value from source)

select *
from renamed
