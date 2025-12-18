{{
    config(
        materialized="table",
        post_hook=[
            "ALTER TABLE {{ this }} ADD PRIMARY KEY (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT character_gender_id FOREIGN KEY (character_gender) REFERENCES {{ ref('dim_character_genders') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT character_species_id FOREIGN KEY (character_species) REFERENCES {{ ref('dim_character_species') }} (id)",
            "ALTER TABLE {{ this }} ADD CONSTRAINT mug_shot_id FOREIGN KEY (mug_shot) REFERENCES {{ ref('dim_character_mug_shots') }} (id)",
        ],
    )
}}

select
    {{
        dbt_utils.star(
            from=source("igdb_raw_v2", "characters"),
            except=["_dlt_load_id", "_dlt_id"],
            quote_identifiers=False,
        )
    }}
from {{ source("igdb_raw_v2", "characters") }}
