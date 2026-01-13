with
    source as (select * from {{ ref("int_popularity_primitives_union") }}),
    renamed as (
        select
            md5(
                game_id::text
                || '-'
                || popularity_type::text
                || '-'
                || to_timestamp(updated_at)::date::text
            ) as id,
            game_id,
            popularity_type,
            popularity_source,
            value,
            calculated_at,
            created_at,
            updated_at,
            external_popularity_source,
            to_timestamp(updated_at) as updated_at_tz,
            to_timestamp(updated_at)::date as snapshot_date
        from source
    )
select *
from renamed
order by updated_at desc
