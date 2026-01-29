with
    source as (select * from {{ ref("stg_game_engines__companies") }}),

    renamed as (select value, _dlt_parent_id, _dlt_list_idx from source)

select *
from renamed
