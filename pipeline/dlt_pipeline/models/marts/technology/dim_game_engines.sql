{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT logo_id FOREIGN KEY (logo) REFERENCES {{ ref('dim_game_engine_logos') }} (id)",
        ],
    )
}}

with
    source as (
        select * from {{ source("igdb_source_20251231072127", "game_engines") }}
    ),

    renamed as (

        select id, created_at, updated_at, name, slug, url, checksum, description,
        from source
    )

select *
from renamed
