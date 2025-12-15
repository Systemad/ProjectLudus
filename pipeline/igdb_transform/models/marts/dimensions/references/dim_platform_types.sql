{{
    config(
        materialized="table",
        post_hook=["ALTER TABLE {{ this }} ADD PRIMARY KEY (id)"],
    )
}}
select {{ dbt_utils.star(from=ref("stg_platform_types"), quote_identifiers=False) }}
from {{ ref("stg_platform_types") }}
