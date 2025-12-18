with source as (

    select * from {{ source('igdb_raw_v2', '_dlt_pipeline_state') }}

),

renamed as (

    select
        version,
        engine_version,
        pipeline_name,
        state,
        created_at,
        version_hash,
        _dlt_load_id,
        _dlt_id

    from source

)

select * from renamed

