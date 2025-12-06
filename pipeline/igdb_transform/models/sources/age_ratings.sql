select
    {{
        dbt_utils.star(
            from(
                ref=("age_ratings"),
                except=["dlt_load_id"],
                quote_identifiers=False,
            )
        )
    }}
from {{ ref("age_ratings") }}
