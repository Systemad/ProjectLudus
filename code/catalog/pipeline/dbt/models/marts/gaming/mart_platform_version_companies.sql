with
source as (select * from {{ ref("stg_platform_version_companies") }}),
companies as (select id from {{ ref("mart_companies") }}),

validated as (

    select
        s.id,
        s.company,
        s.developer,
        s.manufacturer,
        s.checksum,
        s.comment
    from source s
    inner join companies c on s.company = c.id

)

select *
from validated

