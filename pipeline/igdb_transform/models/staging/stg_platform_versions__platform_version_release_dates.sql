with
    source as (

        select *
        from
            {{
                source(
                    "igdb_source_20251229083704",
                    "platform_versions__platform_version_release_dates",
                )
            }}

    ),

    renamed as (select value, _dlt_parent_id, _dlt_list_idx, _dlt_id from source)

select *
from renamed
