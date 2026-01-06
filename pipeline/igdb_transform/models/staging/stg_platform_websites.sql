with
    source as (select * from {{ source("igdb_source2", "platform_websites") }}),

    renamed as (

        select id, trusted, url, checksum, type, _dlt_load_id, _dlt_id from source

    )

select *
from renamed
