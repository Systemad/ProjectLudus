select
    base.id,
    base.created_at,
    base.updated_at,
    base.change_date,
    base.change_date_format,
    base.country,
    base.description,
    base.logo__id as logo,
    base.name,
    base.slug,
    base.start_date,
    base.url,
    base.checksum,
    base.status,
    base.start_date_format,
    case when p.id is not null then base.parent end as parent_id,
    case
        when c.id is not null then base.changed_company_id
    end as changed_company_id
from {{ ref("stg_companies") }} as base
left join {{ ref("stg_companies") }} as p on base.parent = p.id
left join {{ ref("stg_companies") }} as c on base.changed_company_id = c.id
where base.id is not null and base.id != 0
