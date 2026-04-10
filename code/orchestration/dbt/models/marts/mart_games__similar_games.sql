with
    source as (
        select *
        from {{ ref('int_game__similar_game') }}
    )

select *
from source
