with
    source as (select * from {{ ref("int_popularity_primitives_union") }}),
    renamed as (
        select
            md5(
                s.game_id::text
                || '-'
                || s.popularity_type::text
                || '-'
                || to_timestamp(s.updated_at)::date::text
            ) as id,
            s.game_id,
            s.popularity_type,
            s.value,
            s.calculated_at,
            s.created_at,
            s.updated_at,
            s.external_popularity_source,
            to_timestamp(s.updated_at) as updated_at_tz,
            to_timestamp(s.updated_at)::date as snapshot_date
        from source s
        left join {{ ref("mart_games") }} g on s.game_id = g.id
        where g.id is not null
    )
select distinct *
from renamed
order by updated_at desc
