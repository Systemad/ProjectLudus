with
    source as (select * from {{ ref("stg_companies__websites") }}),

    renamed as (

        select id, trusted, url, checksum, type, _dlt_parent_id, _dlt_list_idx

        from source

    )

select *
from renamed
