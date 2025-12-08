-- fmt: off

{% macro create_m2m_relationship(
    parent_resource,
    m2m_field,
    parent_id_column,
    child_id_column,
    child_resource
) %}
    {{
        config(
            materialized="table",
            post_hook=[
                "ALTER TABLE {{ this }} ADD PRIMARY KEY ("~ parent_id_column ~", "~ child_id_column ~")",
                "ALTER TABLE {{ this }} ADD CONSTRAINT fk_"~ parent_id_column ~" FOREIGN KEY ("~ parent_id_column ~ ") REFERENCES {{ ref(parent_resource) }} (id)",
                "ALTER TABLE {{ this }} ADD CONSTRAINT fk_"~ child_id_column ~" FOREIGN KEY ("~ child_id_column ~ ") REFERENCES {{ ref(child_resource) }} (id)",
            ],
        )
    }}


{% set raw_source = source('igdb_raw', parent_resource) %}

select
    t.id as {{ parent_id_column }},
    (jsonb_array_elements_text(t.{{ m2m_field }}::jsonb))::integer as {{ child_id_column }}

from {{ raw_source }} t
where t.{{ m2m_field }} is not null
and t.{{ m2m_field }} != '[]'

{% endmacro %}

/*
{{ create_m2m_relationship(
    parent_resource='games',
    m2m_field='artworks',
    parent_id_column='game_id',
    child_id_column='artwork_id',
    child_resource='artworks'
) }}
*/
