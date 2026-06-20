{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("int_games") }}),

renamed as (

    select
        s.id,
        s.created_at,
        s.updated_at,
        s.cover,
        s.name,
        s.parent_game,
        s.slug,
        s.summary,
        s.url,
        s.checksum,
        s.game_type,
        s.first_release_date as first_release_date_epoch,
        s.rating,
        s.rating_count,
        s.total_rating,
        s.total_rating_count,
        s.storyline,
        s.aggregated_rating,
        s.aggregated_rating_count,
        s.game_status,
        s.hypes,
        s.version_parent,
        s.version_title,
        s.franchise,
        to_timestamp(s.first_release_date::numeric) as first_release_date_utc

    from source as s

)

select *
from renamed
where name is not null and slug is not null and checksum is not null and created_at is not null and updated_at is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}

