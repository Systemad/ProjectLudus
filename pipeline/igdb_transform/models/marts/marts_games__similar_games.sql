with
    source as (select * from {{ ref("stg_games__similar_games") }}),

    renamed as (select value, _dlt_parent_id, _dlt_list_idx from source)

select *
from renamed
