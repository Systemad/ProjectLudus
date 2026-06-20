import os
from datetime import datetime

import pytest
from dlt.common.schema.utils import new_table
from dlt.common.utils import digest128
from dlt_typesense.typesense_client import TypesenseClient

from tests.load.utils import expect_load_file, write_dataset


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_load_job_batching(typesense_client: TypesenseClient, file_storage) -> None:
    """Test that batching works correctly with real Typesense."""
    from io import BytesIO

    # Set batch size to 2 for testing
    typesense_client.config.import_batch_size = 2

    # Create table schema
    table_name = "users"
    table_schema = new_table(
        table_name,
        columns=[
            {"name": "name", "data_type": "text"},
            {"name": "age", "data_type": "bigint"},
        ],
        write_disposition="append",
    )
    typesense_client.schema.update_table(table_schema)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    # Create data with 3 items (batch_size + 1)
    data = [
        {"name": "Alice", "age": 30},
        {"name": "Bob", "age": 25},
        {"name": "Charlie", "age": 35},
    ]

    # Write dataset and load
    with BytesIO() as f:
        write_dataset(typesense_client, f, data, table_schema)
        query = f.getvalue()

    expect_load_file(typesense_client, file_storage, query, table_name)

    qualified_name = typesense_client.make_qualified_collection_name(table_name)
    results = typesense_client._request(
        "GET",
        f"/collections/{qualified_name}/documents/search",
        params={"q": "*", "per_page": 250},
    )

    hits = results.get("hits", [])
    documents = [hit["document"] for hit in hits]

    assert len(documents) == 3, f"Expected 3 documents, got {len(documents)}"

    names = {doc.get("name") for doc in documents}
    assert "Alice" in names
    assert "Bob" in names
    assert "Charlie" in names


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_load_job_transformations(
    typesense_client: TypesenseClient, file_storage
) -> None:
    """Test that data transformations (timestamps, JSON) work correctly."""
    from io import BytesIO

    table_name = "users"
    table_schema = new_table(
        table_name,
        columns=[
            {"name": "name", "data_type": "text"},
            {"name": "created_at", "data_type": "timestamp"},
            {"name": "meta", "data_type": "json"},
        ],
        write_disposition="append",
    )
    typesense_client.schema.update_table(table_schema)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    # Create data with datetime and nested JSON
    data = [
        {
            "name": "David",
            "created_at": datetime(2023, 1, 1, 12, 0, 0),
            "meta": {"foo": "bar", "nested": {"key": "value"}},
        }
    ]

    # Write dataset and load
    with BytesIO() as f:
        write_dataset(typesense_client, f, data, table_schema)
        query = f.getvalue()

    expect_load_file(typesense_client, file_storage, query, table_name)

    qualified_name = typesense_client.make_qualified_collection_name(table_name)
    results = typesense_client._request(
        "GET",
        f"/collections/{qualified_name}/documents/search",
        params={"q": "*", "per_page": 1},
    )

    hits = results.get("hits", [])
    assert len(hits) > 0

    doc = hits[0]["document"]

    # Timestamp should be int64 (Unix timestamp)
    assert isinstance(doc["created_at"], int)
    assert doc["created_at"] == 1672574400  # 2023-01-01 12:00:00 UTC

    # JSON field should be object
    assert isinstance(doc["meta"], dict)
    assert doc["meta"] == {"foo": "bar", "nested": {"key": "value"}}


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_load_job_array_fields(typesense_client: TypesenseClient, file_storage) -> None:
    """Test that array fields load correctly into Typesense."""
    from io import BytesIO

    table_name = "array_items"
    table_schema = new_table(
        table_name,
        columns=[
            {"name": "tags", "data_type": "text[]"},
            {"name": "scores", "data_type": "double[]"},
            {"name": "counts", "data_type": "bigint[]"},
        ],
        write_disposition="append",
    )
    typesense_client.schema.update_table(table_schema)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    data = [
        {"tags": ["red", "blue"], "scores": [1.5, 2.75], "counts": [1, 2, 3]},
    ]

    with BytesIO() as f:
        write_dataset(typesense_client, f, data, table_schema)
        query = f.getvalue()

    expect_load_file(typesense_client, file_storage, query, table_name)

    qualified_name = typesense_client.make_qualified_collection_name(table_name)
    results = typesense_client._request(
        "GET",
        f"/collections/{qualified_name}/documents/search",
        params={"q": "*", "per_page": 1},
    )

    hits = results.get("hits", [])
    assert len(hits) == 1

    doc = hits[0]["document"]
    assert doc["tags"] == ["red", "blue"]
    assert doc["scores"] == [1.5, 2.75]
    assert doc["counts"] == [1, 2, 3]


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_load_job_deterministic_id(
    typesense_client: TypesenseClient, file_storage
) -> None:
    """Test ID handling: preserve existing id, generate deterministic id when missing."""
    from io import BytesIO

    table_name = "users"
    table_schema = new_table(
        table_name,
        columns=[
            {"name": "id", "data_type": "text", "primary_key": True},
            {"name": "name", "data_type": "text"},
        ],
        write_disposition="merge",
    )
    typesense_client.schema.update_table(table_schema)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    data = [{"id": "user1", "name": "Eve"}]

    with BytesIO() as f:
        write_dataset(typesense_client, f, data, table_schema)
        query = f.getvalue()

    expect_load_file(typesense_client, file_storage, query, table_name)

    qualified_name = typesense_client.make_qualified_collection_name(table_name)
    results = typesense_client._request(
        "GET",
        f"/collections/{qualified_name}/documents/search",
        params={"q": "*", "per_page": 1},
    )

    hits = results.get("hits", [])
    assert len(hits) > 0
    doc = hits[0]["document"]
    assert doc["id"] == "user1"

    table_schema2 = new_table(
        "users2",
        columns=[
            {"name": "user_id", "data_type": "text", "primary_key": True},
            {"name": "name", "data_type": "text"},
        ],
        write_disposition="merge",
    )
    typesense_client.schema.update_table(table_schema2)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    data2 = [{"user_id": "alice123", "name": "Alice"}]

    with BytesIO() as f:
        write_dataset(typesense_client, f, data2, table_schema2)
        query = f.getvalue()

    expect_load_file(typesense_client, file_storage, query, "users2")

    qualified_name2 = typesense_client.make_qualified_collection_name("users2")
    results2 = typesense_client._request(
        "GET",
        f"/collections/{qualified_name2}/documents/search",
        params={"q": "*", "per_page": 1},
    )

    hits2 = results2.get("hits", [])
    assert len(hits2) > 0
    doc2 = hits2[0]["document"]

    expected_id = digest128("alice123")
    assert doc2["id"] == expected_id


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_load_job_failure(typesense_client: TypesenseClient, file_storage) -> None:
    """Test error handling with real Typesense errors."""
    from io import BytesIO

    from dlt_typesense.exceptions import (
        TypesenseBulkImportError,
    )

    table_name = "users"
    table_schema = new_table(
        table_name,
        columns=[
            {"name": "age", "data_type": "bigint"},
        ],
        write_disposition="append",
    )
    typesense_client.schema.update_table(table_schema)
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    data = [{"age": "not_a_number"}]

    with BytesIO() as f:
        write_dataset(typesense_client, f, data, table_schema)
        query = f.getvalue()

    # The load job should fail with TypesenseBulkImportError due to invalid data type
    with pytest.raises((TypesenseBulkImportError, AssertionError)):
        expect_load_file(
            typesense_client, file_storage, query, table_name, status="completed"
        )
