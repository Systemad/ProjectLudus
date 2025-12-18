with source as (

    select * from {{ source('igdb_raw_v2', 'platform_version_companies') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        company,
        developer,
        manufacturer,
        checksum,
        _dlt_load_id,
        _dlt_id,
        comment

    from source

)

select * from renamed

