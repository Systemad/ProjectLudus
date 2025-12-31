with
    source as (

        select *
        from
            {{
                source(
                    "igdb_source_20251231072127", "age_rating_content_descriptions_v2"
                )
            }}

    ),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            description,
            organization,
            checksum,
            description_type,
            _dlt_load_id,
            _dlt_id

        from source

    )

select *
from renamed
