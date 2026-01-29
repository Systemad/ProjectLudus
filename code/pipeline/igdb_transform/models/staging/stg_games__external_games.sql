with
    source as (select * from {{ source("igdb_source", "games__external_games") }}),

    renamed as (

        select
            id,
            created_at,
            game,
            name,
            uid,
            updated_at,
            url,
            checksum,
            external_game_source,
            _dlt_parent_id,
            _dlt_list_idx,
            _dlt_id,
            year,
            category,
            media,
            platform,
            countries,
            game_release_format

        from source

    )

select *
from renamed
