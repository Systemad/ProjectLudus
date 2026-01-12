with
    source as (select * from {{ source("igdb_source", "platforms__websites") }}),

    renamed as (select value, _dlt_parent_id, _dlt_list_idx, _dlt_id from source)

select *
from renamed
