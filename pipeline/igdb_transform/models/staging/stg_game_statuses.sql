with source as (

    select * from {{ source('igdb_raw_v2', 'game_statuses') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        status,
        checksum,
        _dlt_load_id,
        _dlt_id

    from source

)

select * from renamed

