with
formatted as (
    select
        id,
        game,
        name,
        video_id,
        checksum
    from {{ ref("stg_videos") }}
)

select *
from formatted
where name is not null and checksum is not null

