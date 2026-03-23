with
    source as (select * from {{ ref("stg_games__standalone_expansions") }}),

    renamed as (select value from source)

select *
from renamed
