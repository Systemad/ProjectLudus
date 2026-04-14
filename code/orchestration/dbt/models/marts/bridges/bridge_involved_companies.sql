with

    formatted as (

        select
            id,
            company,
            created_at,
            developer,
            game,
            porting,
            publisher,
            supporting,
            updated_at,
            checksum
        from {{ ref("stg_involved_companies") }}

    )

select *
from formatted
where game is not null
    and company is not null
