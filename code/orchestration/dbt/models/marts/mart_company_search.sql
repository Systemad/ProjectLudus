{{ config(materialized="table") }}

with
    company_game_counts as (
        select
            ic.company as company_id,
            count(distinct case when ic.developer then ic.game end)::int
            as games_developed_count,
            count(distinct case when ic.publisher then ic.game end)::int
            as games_published_count
        from {{ ref("bridge_involved_companies") }} ic
        group by ic.company
    )

select
    c.id,
    c.name,
    c.description,
    c.slug,
    c.url,
    coalesce(c.updated_at, 0)::bigint as updated_at,
    c.start_date::bigint as start_date,
    case
        when c.start_date is not null and c.start_date > 0
        then extract(year from to_timestamp(c.start_date::numeric))::int
        else null
    end as start_year,
    cp.name as parent_company,
    cc.name as changed_company,
    cs.name as status,
    cl.image_id as logo_url,
    coalesce(cgc.games_developed_count, 0) as games_developed_count,
    coalesce(cgc.games_published_count, 0) as games_published_count
from {{ ref("mart_companies") }} c
left join {{ ref("mart_company_statuses") }} cs on c.status = cs.id
left join {{ ref("mart_company_logos") }} cl on c.logo = cl.id
left join {{ ref("mart_companies") }} cp on c.parent_id = cp.id
left join {{ ref("mart_companies") }} cc on c.changed_company_id = cc.id
left join company_game_counts cgc on c.id = cgc.company_id
