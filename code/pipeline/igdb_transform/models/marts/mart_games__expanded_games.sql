with
    source as (select * from {{ ref("stg_games__expanded_games") }}),

    renamed as (select value from source)

select *
from renamed
