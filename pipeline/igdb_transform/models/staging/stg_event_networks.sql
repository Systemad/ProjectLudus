with source as (

    select * from {{ source('igdb_raw_v2', 'event_networks') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        event,
        url,
        network_type,
        checksum,
        _dlt_load_id,
        _dlt_id

    from source

)

select * from renamed

