with source as (

    select * from {{ source('igdb_raw_v2', 'event_logos') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        event,
        alpha_channel,
        animated,
        height,
        image_id,
        url,
        width,
        checksum,
        _dlt_load_id,
        _dlt_id

    from source

)

select * from renamed

