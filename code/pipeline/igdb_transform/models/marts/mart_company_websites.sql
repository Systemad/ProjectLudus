with
    formatted as (

        select id, trusted, url, checksum, type from {{ ref("stg_company_websites") }}
    )
select *
from formatted
