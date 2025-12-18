with source as (

    select * from {{ source('igdb_raw_v2', '_dlt_loads') }}

),

renamed as (

    select
        load_id,
        schema_name,
        status,
        inserted_at,
        schema_version_hash

    from source

)

select * from renamed

