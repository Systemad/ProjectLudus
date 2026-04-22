with source as (
    select * from {{ source('igdb_source', 'pop_steam_24hr_peak_players') }}
),
renamed as (
    select
        id,
        game_id,
        popularity_type,
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
