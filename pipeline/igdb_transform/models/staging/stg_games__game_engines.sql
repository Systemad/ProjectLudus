with
    source as (

        select * from {{ source("igdb_source_20251231072127", "games__game_engines") }}

    ),

    renamed as (select value, _dlt_parent_id, _dlt_list_idx, _dlt_id from source)

select *
from renamed
