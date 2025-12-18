with source as (

    select * from {{ source('igdb_raw_v2', 'age_rating_content_descriptions_v2') }}

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

select * from renamed

