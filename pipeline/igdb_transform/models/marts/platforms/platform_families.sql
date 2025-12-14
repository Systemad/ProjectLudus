{{
    config(
        materialized="table",
        post_hook=["ALTER TABLE {{ this }} ADD PRIMARY KEY (id)"],
    )
}}
select
    {{
        dbt_utils.star(
            from=ref("stg_platform_family"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ ref("stg_platform_family") }}
