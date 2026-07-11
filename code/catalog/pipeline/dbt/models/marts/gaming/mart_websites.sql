with

formatted as (

    select
        w.id,
        w.game,
        w.trusted,
        w.url,
        w.checksum,
        w.type
    from {{ ref("stg_websites") }} w
    inner join {{ ref("mart_games") }} g on w.game = g.id

)

select *
from formatted

