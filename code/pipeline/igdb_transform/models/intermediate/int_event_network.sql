select
    base.id,
    base.created_at,
    base.updated_at,
    base.event,
    base.url,
    base.network_type,
    base.checksum
from {{ ref("stg_event_networks") }} base
inner join {{ ref("mart_events") }} e on base.event = e.id
where base.id is not null and base.id != 0
