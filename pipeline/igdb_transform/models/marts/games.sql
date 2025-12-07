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
select
    {{
        dbt_utils.star(
            from=source("igdb_raw", "games"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw", "games") }}
