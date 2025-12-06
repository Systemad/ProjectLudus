with
    source as (select * from {{ source("igdb_raw", "themes") }}),
    renamed as (
        select
            {{ adapter.quote("id") }},
            {{ adapter.quote("created_at") }},
            {{ adapter.quote("updated_at") }},
            {{ adapter.quote("name") }},
            {{ adapter.quote("slug") }},
            {{ adapter.quote("url") }},
            {{ adapter.quote("checksum") }},
            {{ adapter.quote("_dlt_load_id") }},
            {{ adapter.quote("_dlt_id") }}

        from source
    )
select *
from renamed
