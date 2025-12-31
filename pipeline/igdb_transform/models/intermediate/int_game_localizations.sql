select
    base.id,
    base.name,
    base.game,
    base.region,
    base.created_at,
    base.updated_at,
    base.checksum,
    base.cover,
    case when c.id is not null then base.cover else null end as cover_id
from {{ ref("stg_game_localizations") }} base
left join {{ ref("stg_covers") }} c on base.cover = c.id
where base.id is not null and base.id != 0
