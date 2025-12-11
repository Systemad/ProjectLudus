{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT franchise_id FOREIGN KEY (franchise) REFERENCES {{ ref('franchises') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_status_id FOREIGN KEY (game_status) REFERENCES {{ ref('game_statuses') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_type_id FOREIGN KEY (game_types) REFERENCES {{ ref('game_types') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT parent_game_id FOREIGN KEY (parent_game) REFERENCES {{ this }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT version_parent_id FOREIGN KEY (version_parent) REFERENCES {{ this }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT cover_id FOREIGN KEY (cover) REFERENCES {{ ref('covers') }} (id)",
        ],
    )
}}

with
    source as (select * from {{ source("igdb_raw_new", "games") }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            name,
            parent_game,
            slug,
            summary,
            url,
            checksum,
            game_type,
            first_release_date,
            rating,
            rating_count,
            storyline,
            total_rating,
            total_rating_count,
            hypes,
            status,
            game_status,
            aggregated_rating,
            aggregated_rating_count,
            version_parent,
            version_title,
            franchise

        from source

    )

select *
from renamed
