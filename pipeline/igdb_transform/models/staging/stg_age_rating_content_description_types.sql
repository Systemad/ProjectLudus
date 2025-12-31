with
    source as (

        select *
        from
            {{
                source(
                    "igdb_source_20251231072127",
                    "age_rating_content_description_types",
                )
            }}

    ),

    renamed as (

        select id, created_at, updated_at, slug, name, checksum, _dlt_load_id, _dlt_id

        from source

    )

select *
from renamed
