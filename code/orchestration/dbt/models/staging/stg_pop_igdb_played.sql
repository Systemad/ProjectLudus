with source as (
    select * from {{ source('igdb_source', 'pop_igdb_played') }}
),
renamed as (
    select
        id,
        game_id,
        popularity_type,
        popularity_source,
        value,
        calculated_at,
        created_at,
        updated_at,
        external_popularity_source,
        _dlt_load_id,
        _dlt_id
    from source
)
select * from renamed
