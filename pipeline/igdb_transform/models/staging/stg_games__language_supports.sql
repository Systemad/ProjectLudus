with source as (

    select * from {{ source('igdb_raw_v2', 'games__language_supports') }}

),

renamed as (

    select
        id,
        game,
        language,
        language_support_type,
        created_at,
        updated_at,
        checksum,
        _dlt_parent_id,
        _dlt_list_idx,
        _dlt_id

    from source

)

select * from renamed

