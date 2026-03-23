with
    source as (select * from {{ ref("stg_games__remasters") }}),

    renamed as (select value from source)

select *
from renamed
