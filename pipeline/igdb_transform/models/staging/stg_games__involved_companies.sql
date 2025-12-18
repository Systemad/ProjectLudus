with source as (

    select * from {{ source('igdb_raw_v2', 'games__involved_companies') }}

),

renamed as (

    select
        id,
        company,
        created_at,
        developer,
        game,
        porting,
        publisher,
        supporting,
        updated_at,
        checksum,
        _dlt_parent_id,
        _dlt_list_idx,
        _dlt_id

    from source

)

select * from renamed

