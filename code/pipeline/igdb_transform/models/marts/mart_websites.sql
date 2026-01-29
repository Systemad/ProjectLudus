with

    formatted as (

        select id, game, trusted, url, checksum, type from {{ ref("stg_websites") }}

    )

select *
from formatted
