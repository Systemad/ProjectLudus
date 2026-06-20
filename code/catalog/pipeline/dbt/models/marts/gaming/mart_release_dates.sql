{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

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
from renamed as rd
inner join {{ ref("mart_games") }} as mg on rd.game = mg.id
inner join {{ ref("mart_platforms") }} as mp on rd.platform = mp.id
where
    rd.game is not null
    and rd.platform is not null
{% if is_incremental() %}
and rd.updated_at > (select max(updated_at) from {{ this }})
{% endif %}

