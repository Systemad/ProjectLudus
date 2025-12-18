with source as (

    select * from {{ source('igdb_raw_v2', 'character_genders') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        checksum,
        _dlt_load_id,
        _dlt_id

    from source

)

select * from renamed

