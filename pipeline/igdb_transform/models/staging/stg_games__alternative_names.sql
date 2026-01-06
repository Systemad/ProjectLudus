with
    source as (select * from {{ source("igdb_source2", "games__alternative_names") }}),

    renamed as (

        select id, comment, game, name, checksum, _dlt_parent_id, _dlt_list_idx, _dlt_id

        from source

    )

select *
from renamed
