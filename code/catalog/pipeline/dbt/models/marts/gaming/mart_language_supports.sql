{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_language_supports") }}),
games as (select id from {{ ref("mart_games") }}),

renamed as (

    select
        s.id,
        s.game,
        s.language,
        s.language_support_type,
        s.created_at,
        s.updated_at,
        s.checksum

    from source s
    inner join games g on s.game = g.id

)

select *
from renamed
where id is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}


