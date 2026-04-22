with
    formatted as (

        select
            id,
            created_at,
            game,
            name,
            uid,
            updated_at,
            url,
            checksum,
            year,
            platform,
            coalesce(countries, '[]'::jsonb) as countries,
            external_game_source,
            game_release_format

        from {{ ref("int_external_games") }}

    ),

    deduplicated as (
        {{
            dbt_utils.deduplicate(
                relation="formatted",
                partition_by="id",
                order_by="updated_at desc, created_at desc",
            )
        }}

    )

select *
from deduplicated
