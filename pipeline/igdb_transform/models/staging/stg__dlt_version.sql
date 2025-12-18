with source as (

    select * from {{ source('igdb_raw_v2', '_dlt_version') }}

),

renamed as (

    select
        version,
        engine_version,
        inserted_at,
        schema_name,
        version_hash,
        schema

    from source

)

select * from renamed

