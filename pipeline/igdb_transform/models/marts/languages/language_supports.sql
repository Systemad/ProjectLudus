{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT game_id FOREIGN KEY (game) REFERENCES {{ ref('games') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT language_id FOREIGN KEY (language) REFERENCES {{ ref('languages') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT language_support_type_id FOREIGN KEY (language_support_type) REFERENCES {{ ref('language_support_types') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_v2", "games__language_supports"),
            except=["_dlt_parent_id", "_dlt_list_idx", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_v2", "games__language_supports") }}
