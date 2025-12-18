with source as (

    select * from {{ source('igdb_raw_v2', 'companies__websites') }}

),

renamed as (

    select
        id,
        trusted,
        url,
        checksum,
        type,
        _dlt_parent_id,
        _dlt_list_idx,
        _dlt_id

    from source

)

select * from renamed

