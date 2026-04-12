with
    source as (select * from {{ ref("stg_release_dates") }}),

    renamed as (

        select
            id,
            created_at,
            date,
            game,
            human,
            m,
            platform,
            updated_at,
            y,
            checksum,
            status,
            date_format,
            release_region

        from source

    )

select rd.*
from renamed rd
inner join {{ ref("mart_games") }} mg on rd.game = mg.id
inner join {{ ref("mart_platforms") }} mp on rd.platform = mp.id
where rd.game is not null
    and rd.platform is not null
