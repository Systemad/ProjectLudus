with
    source as (select * from {{ ref("stg__dlt_pipeline_state") }}),

    renamed as (

        select version, engine_version, pipeline_name, state, created_at, version_hash,
        from source

    )

select *
from renamed
