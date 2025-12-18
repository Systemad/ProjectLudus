with source as (

    select * from {{ source('igdb_raw_v2', 'companies__developed') }}

),

renamed as (

    select
        value,
        _dlt_parent_id,
        _dlt_list_idx,
        _dlt_id

    from source

)

select * from renamed

