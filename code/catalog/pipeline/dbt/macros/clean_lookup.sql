{% macro clean_lookup(table_name, columns, required_cols=[]) %}
with
source as (select * from {{ ref(table_name) }}),

renamed as (

    select
        {% for col in columns %}
        {{ col }}{% if not loop.last %},{% endif %}
        {% endfor %}

    from source

)

select * from renamed
{% for col in required_cols %}
{% if loop.first %}where{% else %}and{% endif %} {{ col }} is not null and {{ col }}::text != ''
{% endfor %}
{% endmacro %}
