{{ config(
    materialized="incremental",
    unique_key="id",
    on_schema_change="append_new_columns"
) }}

with
company_game_counts as (
    select
        ic.company as company_id,
        count(distinct case when ic.developer then ic.game end)::int
            as games_developed_count,
        count(distinct case when ic.publisher then ic.game end)::int
            as games_published_count
    from {{ ref("bridge_involved_companies") }} as ic
    group by ic.company
)

select
    c.id,
    c.name,
    c.description,
    c.slug,
    c.url,
    c.start_date::bigint as start_date,
    cp.name as parent_company,
    cc.name as changed_company,
    cs.name as status,
    cl.image_id as logo_url,
    c.updated_at::bigint as updated_at,
    case
        when c.start_date is not null and c.start_date > 0
            then extract(year from to_timestamp(c.start_date::numeric))::int
    end as start_year,
    coalesce(cgc.games_developed_count, 0) as games_developed_count,
    coalesce(cgc.games_published_count, 0) as games_published_count
from {{ ref("mart_companies") }} as c
left join {{ ref("mart_company_statuses") }} as cs on c.status = cs.id
left join {{ ref("mart_company_logos") }} as cl on c.logo = cl.id
left join {{ ref("mart_companies") }} as cp on c.parent_id = cp.id
left join {{ ref("mart_companies") }} as cc on c.changed_company_id = cc.id
left join company_game_counts as cgc on c.id = cgc.company_id
where c.name is not null and c.slug is not null and c.updated_at is not null
{% if is_incremental() %}
and c.updated_at::bigint > (select max(updated_at) from {{ this }})
{% endif %}

