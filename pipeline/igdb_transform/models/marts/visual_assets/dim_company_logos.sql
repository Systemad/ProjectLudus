{{
    config(
        materialized="table",
        post_hook=["ALTER TABLE {{ this }} ADD PRIMARY KEY (id)"],
    )
}}

select id, company_id, alpha_channel, animated, height, image_id, url, width, checksum
from {{ ref("stg_company_logos") }}
