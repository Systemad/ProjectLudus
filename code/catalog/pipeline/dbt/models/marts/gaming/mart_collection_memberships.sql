{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
source as (select * from {{ ref("stg_collection_memberships") }}),
games as (select id from {{ ref("mart_games") }}),

renamed as (

    select
        s.id,
        s.created_at,
        s.updated_at,
        s.game,
        s.collection,
        s.type,
        s.checksum
    from source s
    inner join games g on s.game = g.id

)

select *
from renamed
{% if is_incremental() %}
where updated_at > (select max(updated_at) from {{ this }})
{% endif %}


