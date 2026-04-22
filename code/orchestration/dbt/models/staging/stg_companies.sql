{% set companies_relation = source("igdb_source", "companies") %}
{% set companies_columns = adapter.get_columns_in_relation(companies_relation) %}
{% set companies_column_names = companies_columns | map(attribute="name") | map("lower") | list %}

with
    source as (select * from {{ companies_relation }}),

    renamed as (

        select
            id,
            created_at,
            updated_at,
            change_date,
            {% if "change_date_format" in companies_column_names %}
            change_date_format,
            {% else %}
            null::bigint as change_date_format,
            {% endif %}
            country,
            description,
            logo__id,
            logo__alpha_channel,
            logo__animated,
            logo__height,
            logo__image_id,
            logo__url,
            logo__width,
            logo__checksum,
            name,
            slug,
            start_date,
            url,
            checksum,
            status,
            {% if "start_date_format" in companies_column_names %}
            start_date_format,
            {% else %}
            null::bigint as start_date_format,
            {% endif %}
            _dlt_load_id,
            _dlt_id,
            nullif(parent, 0)::bigint as parent,
            nullif(changed_company_id, 0)::bigint as changed_company_id
        from source

    )

select *
from renamed
