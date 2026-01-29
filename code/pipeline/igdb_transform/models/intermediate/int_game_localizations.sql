select
    base.id,
    base.name,
    base.game,
    base.region,
    base.created_at,
    base.updated_at,
    base.checksum,
    c.id as cover
from {{ ref("stg_game_localizations") }} base
left join {{ ref("int_covers_unified") }} c on base.cover = c.id
where base.id is not null and base.id != 0
