with source as (

    select * from {{ source('igdb_raw_v2', 'collection_membership_types') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        description,
        allowed_collection_type,
        checksum,
        _dlt_load_id,
        _dlt_id

    from source

)

select * from renamed

