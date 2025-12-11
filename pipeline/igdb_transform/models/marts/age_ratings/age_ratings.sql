{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT rating_category_id FOREIGN KEY (rating_category) REFERENCES {{ ref('age_rating_categories') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT organization_id FOREIGN KEY (organization) REFERENCES {{ ref('age_rating_organizations') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_new", "games__age_ratings"),
            except=["_dlt_parent_id", "_dlt_list_idx", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_new", "games__age_ratings") }}
