import os
import pytest
from dlt.common.schema.utils import new_table
from dlt_typesense.typesense_client import TypesenseClient
from tests.utils import get_collection_schema, assert_collection_schema
from tests.conftest import typesense_client, file_storage
from tests.pipeline.utils import assert_load_info
import dlt
from dlt.common.utils import uniq_id


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_create_collection(typesense_client: TypesenseClient) -> None:
    """Test collection creation with real Typesense."""
    table_name = "users"

    # Setup schema table
    table_schema = new_table(
        table_name,
        columns=[
            {"name": "name", "data_type": "text"},
            {"name": "age", "data_type": "bigint", "x-typesense-facet": True},
        ],
        write_disposition="append",
    )
    typesense_client.schema.update_table(table_schema)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    # Verify collection was created in Typesense
    qualified_name = typesense_client.make_qualified_collection_name(table_name)
    collection = typesense_client._request("GET", f"/collections/{qualified_name}")
    assert collection is not None, f"Collection {qualified_name} was not created"

    # Get actual schema from Typesense
    actual_fields = get_collection_schema(typesense_client, table_name)
    assert actual_fields is not None

    # Verify fields
    field_map = {f["name"]: f for f in actual_fields}

    # Check name field
    assert "name" in field_map
    name_field = field_map["name"]
    assert name_field["type"] == "string"

    assert "age" in field_map
    age_field = field_map["age"]
    assert age_field["type"] == "int64"
    assert age_field.get("facet") is True


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_update_collection_schema(
    typesense_client: TypesenseClient, file_storage
) -> None:
    """Test schema updates with real Typesense."""
    table_name = "users"

    # Create initial schema
    initial_schema = new_table(
        table_name,
        columns=[
            {"name": "name", "data_type": "text"},
        ],
        write_disposition="append",
    )
    typesense_client.schema.update_table(initial_schema)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    # Verify initial collection exists
    qualified_name = typesense_client.make_qualified_collection_name(table_name)
    collection = typesense_client._request("GET", f"/collections/{qualified_name}")
    assert collection is not None

    initial_fields = get_collection_schema(typesense_client, table_name)
    assert initial_fields is not None
    initial_field_names = {f["name"] for f in initial_fields}
    assert "name" in initial_field_names
    assert "email" not in initial_field_names

    # Update schema with new field
    updated_schema = new_table(
        table_name,
        columns=[
            {"name": "name", "data_type": "text"},
            {"name": "email", "data_type": "text"},
        ],
        write_disposition="append",
    )
    typesense_client.schema.update_table(updated_schema)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    updated_fields = get_collection_schema(typesense_client, table_name)
    assert updated_fields is not None
    updated_field_names = {f["name"] for f in updated_fields}
    assert "name" in updated_field_names
    assert "email" in updated_field_names

    field_map = {f["name"]: f for f in updated_fields}
    email_field = field_map["email"]
    assert email_field["type"] == "string"

    from io import BytesIO
    from tests.load.utils import write_dataset, expect_load_file

    data = [{"name": "Bob", "email": "bob@example.com"}]

    with BytesIO() as f:
        write_dataset(typesense_client, f, data, updated_schema)
        query = f.getvalue()

    expect_load_file(typesense_client, file_storage, query, table_name)

    results = typesense_client._request(
        "GET",
        f"/collections/{typesense_client.make_qualified_collection_name(table_name)}/documents/search",
        params={"q": "*", "per_page": 1},
    )

    hits = results.get("hits", [])
    assert len(hits) > 0
    doc = hits[0]["document"]
    assert doc.get("name") == "Bob"
    assert doc.get("email") == "bob@example.com"
