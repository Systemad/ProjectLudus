with

    formatted as (

        select id, comment, game, name, checksum from {{ ref("stg_alternative_names") }}

    )

select *
from formatted
