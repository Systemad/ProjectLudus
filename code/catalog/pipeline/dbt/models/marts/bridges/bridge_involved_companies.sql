{{ config(
    materialized="incremental",
    unique_key=["id"],
    on_schema_change="append_new_columns"
) }}

with

formatted as (

    select
        i.id,
        i.company,
        i.created_at,
        i.developer,
        i.game,
        i.porting,
        i.publisher,
        i.supporting,
        i.updated_at,
        i.checksum
    from {{ ref("stg_involved_companies") }} as i
    inner join {{ ref("mart_companies") }} as c on i.company = c.id

)

select *
from formatted
where
    game is not null
    and company is not null
{% if is_incremental() %}
and updated_at > (select max(updated_at) from {{ this }})
{% endif %}
