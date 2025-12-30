select
    base.id,
    base.created_at,
    base.updated_at,
    base.name,
    base.slug,
    base.summary,
    base.url,
    base.checksum,
    base.cpu,
    base.media,
    base.memory,
    base.output,
    base.resolutions,
    base.sound,
    base.connectivity,
    base.storage,
    base.graphics,
    base.os,

    case when p.id is not null then base.platform_logo else null end as platform_logo,
    case
        when pm.id is not null then base.main_manufacturer else null
    end as main_manufacturer
from {{ ref("stg_platform_versions") }} base
left join {{ ref("stg_platform_logo") }} p on base.platform_logo = p.id
left join
    {{ ref("stg_platform_version_companies") }} pm on base.main_manufacturer = pm.id
where base.id is not null and base.id != 0
