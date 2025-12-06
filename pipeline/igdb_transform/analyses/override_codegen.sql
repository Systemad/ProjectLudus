{% macro create_base_model(table_name) %}
    {{ config(materialized="table") }}

    select
        {{
            dbt_utils.star(
                from(
                    ref=(table_name),
                    except=["dlt_load_id"],
                    quote_identifiers=False,
                )
            )
        }}
    from {{ ref(table_name) }}
{% endmacro %}
