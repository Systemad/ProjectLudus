with source as (

    select * from {{ source('igdb_raw_v2', 'game_version_feature_values') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        game,
        game_feature,
        included_feature,
        checksum,
        _dlt_load_id,
        _dlt_id,
        note

    from source

)

select * from renamed

