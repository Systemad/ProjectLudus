with source as (

    select * from {{ source('igdb_raw_v2', 'collection_types') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        description,
        checksum,
        _dlt_load_id,
        _dlt_id

    from source

)

select * from renamed

