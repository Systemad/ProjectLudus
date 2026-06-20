import os
import pytest
from typing import Iterator
import dlt
from dlt.common.utils import uniq_id
from dlt_typesense.typesense_adapter import typesense_adapter
from dlt_typesense.typesense_client import TypesenseClient
from tests.pipeline.utils import assert_load_info
from tests.utils import drop_active_pipeline_data, assert_collection

# Mark all tests as essential
pytestmark = pytest.mark.essential


@pytest.fixture(autouse=True)
def drop_typesense_data() -> Iterator[None]:
    """Auto-cleanup after each test."""
    yield
    drop_active_pipeline_data()


def test_basic_pipeline_append() -> None:
    """Test basic append write disposition."""
    data = [
        {"id": "1", "name": "Alice", "age": 30},
        {"id": "2", "name": "Bob", "age": 25},
    ]

    @dlt.resource
    def users():
        yield data

    pipeline = dlt.pipeline(
        pipeline_name="test_append_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    info = pipeline.run(users())
    assert_load_info(info)

    # Verify data in Typesense
    assert_collection(pipeline, "users", expected_items_count=2, items=data)


def test_pipeline_replace() -> None:
    """Test replace write disposition."""

    @dlt.resource
    def users():
        yield [{"id": "1", "name": "Alice"}, {"id": "2", "name": "Bob"}]

    pipeline = dlt.pipeline(
        pipeline_name="test_replace_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    # First load
    info = pipeline.run(users(), write_disposition="replace")
    assert_load_info(info)
    assert_collection(pipeline, "users", expected_items_count=2)

    # Second load (should replace)
    new_data = [{"id": "3", "name": "Charlie"}]

    @dlt.resource(table_name="users")
    def new_users():
        yield new_data

    info = pipeline.run(new_users(), write_disposition="replace")
    assert_load_info(info)
    assert_collection(pipeline, "users", expected_items_count=1, items=new_data)


def test_pipeline_merge() -> None:
    """Test merge (upsert) write disposition."""
    data = [
        {"id": "1", "name": "Alice", "age": 30},
        {"id": "2", "name": "Bob", "age": 25},
    ]

    @dlt.resource(primary_key="id")
    def users():
        yield data

    pipeline = dlt.pipeline(
        pipeline_name="test_merge_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    # First load
    info = pipeline.run(users(), write_disposition="merge")
    assert_load_info(info)
    assert_collection(pipeline, "users", expected_items_count=2, items=data)

    # Update existing + add new
    updated_data = [
        {"id": "1", "name": "Alice Updated", "age": 31},  # Updated
        {"id": "3", "name": "Charlie", "age": 35},  # New
    ]

    @dlt.resource(primary_key="id", table_name="users")
    def updated_users():
        yield updated_data

    info = pipeline.run(updated_users(), write_disposition="merge")
    assert_load_info(info)

    # Should have 3 total (1 updated, 1 unchanged, 1 new)
    expected = [
        {"id": "1", "name": "Alice Updated", "age": 31},
        {"id": "2", "name": "Bob", "age": 25},
        {"id": "3", "name": "Charlie", "age": 35},
    ]
    assert_collection(pipeline, "users", expected_items_count=3, items=expected)


def test_schema_evolution() -> None:
    """Test adding new columns to existing collection."""

    @dlt.resource
    def users():
        yield [{"id": "1", "name": "Alice"}]

    pipeline = dlt.pipeline(
        pipeline_name="test_schema_evolution_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    # First load
    pipeline.run(users())

    # Second load with new column
    @dlt.resource(table_name="users")
    def users_with_email():
        yield [{"id": "2", "name": "Bob", "email": "bob@example.com"}]

    pipeline.run(users_with_email())

    # Verify schema updated
    assert "email" in pipeline.default_schema.tables["users"]["columns"]

    # Verify both documents exist
    assert_collection(pipeline, "users", expected_items_count=2)


def test_adapter_hints() -> None:
    """Test typesense_adapter with facet, sort, index hints - verify in Typesense schema."""

    @dlt.resource
    def products():
        yield [
            {"id": "1", "name": "Product A", "price": 100, "category": "Electronics"},
            {"id": "2", "name": "Product B", "price": 200, "category": "Books"},
        ]

    # Apply adapter hints
    products_r = typesense_adapter(
        products,
        facet=["category", "price"],
        sort=["price", "name"],
    )

    pipeline = dlt.pipeline(
        pipeline_name="test_adapter_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    info = pipeline.run(products_r())
    assert_load_info(info)

    # Verify hints applied in dlt schema
    table_schema = pipeline.default_schema.tables["products"]
    assert table_schema["columns"]["category"].get("x-typesense-facet") is True
    assert table_schema["columns"]["price"].get("x-typesense-facet") is True
    assert table_schema["columns"]["price"].get("x-typesense-sort") is True

    # Verify hints are actually in Typesense collection schema
    from tests.utils import get_collection_schema

    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        fields = get_collection_schema(client, "products")
        assert fields is not None

        field_map = {f["name"]: f for f in fields}

        # Verify category has facet enabled
        assert "category" in field_map
        assert field_map["category"].get("facet") is True

        # Verify price has facet and sort enabled
        assert "price" in field_map
        assert field_map["price"].get("facet") is True
        assert field_map["price"].get("sort") is True

        # Verify name has sort enabled
        assert "name" in field_map
        assert field_map["name"].get("sort") is True


def test_state_and_schema_storage() -> None:
    """Test that state and schema are stored correctly."""

    @dlt.resource
    def users():
        yield [{"id": "1", "name": "Alice"}]

    pipeline = dlt.pipeline(
        pipeline_name="test_state_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    info = pipeline.run(users())
    assert_load_info(info)

    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        # Check stored schema
        schema = client.get_stored_schema(client.schema.name)
        assert schema is not None
        assert schema.schema_name == client.schema.name

        # Check stored state
        state = client.get_stored_state(pipeline.pipeline_name)
        assert state is not None
        assert state.pipeline_name == pipeline.pipeline_name


def test_timestamp_conversion() -> None:
    """Test that timestamps are converted to int64."""
    from datetime import datetime

    @dlt.resource
    def events():
        yield [
            {
                "id": "1",
                "name": "Event 1",
                "created_at": datetime(2023, 1, 1, 12, 0, 0),
            }
        ]

    pipeline = dlt.pipeline(
        pipeline_name="test_timestamps_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    info = pipeline.run(events())
    assert_load_info(info)

    # Verify timestamp was converted (check via Typesense API)
    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        qualified_name = client.make_qualified_collection_name("events")
        results = client._request(
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


def test_empty_dataset() -> None:
    """Test that dataset_name is optional and client handles None correctly."""

    @dlt.resource
    def users():
        yield [{"id": "1", "name": "Alice"}]

    pipeline = dlt.pipeline(
        pipeline_name="test_empty_dataset_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dev_mode=True,
        dataset_name=None,  # Explicitly set to None
    )

    info = pipeline.run(users())
    assert_load_info(info)

    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        # Client should handle None dataset_name correctly
        # If dataset_name is None, sentinel_collection should use fallback
        if client.dataset_name is None:
            assert client.sentinel_collection == "DltSentinelCollection"
        else:
            # If dlt auto-generated a dataset name (e.g., in dev_mode), that's also valid
            assert client.sentinel_collection is not None

        # Verify sentinel collection exists
        sentinel = client._request("GET", f"/collections/{client.sentinel_collection}")
        assert sentinel is not None, "Sentinel collection should exist"

        # Verify collection name doesn't have dataset prefix when dataset_name is None
        qualified_name = client.make_qualified_collection_name("users")
        if client.dataset_name is None:
            assert (
                qualified_name == "users"
            ), "Collection name should not be prefixed when dataset_name is None"
        else:
            assert (
                client.dataset_name in qualified_name
            ), "Collection name should be prefixed when dataset_name is set"

    # Collection should exist (with or without dataset prefix)
    assert_collection(pipeline, "users", expected_items_count=1)

    # Test that multiple pipelines with None dataset_name don't conflict
    pipeline2 = dlt.pipeline(
        pipeline_name="test_empty_dataset_2_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dev_mode=True,
        dataset_name=None,
    )

    @dlt.resource
    def users2():
        yield [{"id": "2", "name": "Bob"}]

    info2 = pipeline2.run(users2())
    assert_load_info(info2)

    assert_collection(pipeline, "users", expected_items_count=1)

    with pipeline2.destination_client() as client2:  # type: ignore[assignment]
        qualified_name2 = client2.make_qualified_collection_name("users")
        collection2 = client2._request("GET", f"/collections/{qualified_name2}")
        if collection2 is not None:
            assert_collection(pipeline2, "users", expected_items_count=1)


def test_merge_github_nested() -> None:
    """Test merge disposition with complex nested GitHub issues data."""
    # Create test data that mimics GitHub issues with nested structures
    # (labels and assignees will be normalized into separate tables)
    data = [
        {
            "id": i,
            "title": f"Issue {i}",
            "body": f"Description for issue {i}",
            "state": "open" if i % 2 == 0 else "closed",
            "labels": [{"name": f"label_{j}"} for j in range(i % 3 + 1)],
            "assignees": [{"login": f"user_{j}"} for j in range(i % 2 + 1)],
        }
        for i in range(1, 18)  # 17 items to match original test
    ]

    p = dlt.pipeline(
        destination="dlt_typesense.factory.typesense",
        dataset_name="github1",
        dev_mode=True,
    )
    assert p.dataset_name.startswith("github1_202")

    info = p.run(
        data,
        table_name="issues",
        write_disposition="merge",
        primary_key="id",
    )
    assert_load_info(info)

    # assert if schema contains tables with right names
    # Typesense uses snake_case naming convention
    assert set(p.default_schema.tables.keys()) == {
        "_dlt_version",
        "_dlt_loads",
        "issues",
        "_dlt_pipeline_state",
        "issues__labels",
        "issues__assignees",
    }
    assert set([t["name"] for t in p.default_schema.data_tables()]) == {
        "issues",
        "issues__labels",
        "issues__assignees",
    }
    assert set([t["name"] for t in p.default_schema.dlt_tables()]) == {
        "_dlt_version",
        "_dlt_loads",
        "_dlt_pipeline_state",
    }
    issues = p.default_schema.tables["issues"]
    assert issues["columns"]["id"]["primary_key"] is True
    assert_collection(p, "issues", expected_items_count=17)


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_authentication_error() -> None:
    """Test that wrong API key raises TypesenseApiError."""
    from dlt_typesense.exceptions import TypesenseApiError
    from dlt_typesense.configuration import TypesenseCredentials
    from dlt_typesense.factory import typesense
    from dlt.common.schema import Schema

    original_url = os.environ.get(
        "DESTINATION__TYPESENSE__CREDENTIALS__URL", "http://localhost:8108"
    )

    wrong_config = typesense().spec()
    wrong_config.credentials = TypesenseCredentials(
        url=original_url, api_key="wrong_key_12345"
    )
    wrong_config = wrong_config._bind_dataset_name(dataset_name="test_dataset")

    wrong_client = typesense().client(Schema("test"), wrong_config)
    with wrong_client:
        with pytest.raises(TypesenseApiError):
            wrong_client._request(
                "POST", "/collections", json_data={"name": "test", "fields": []}
            )


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_missing_collection_error() -> None:
    """Test that accessing non-existent collection returns None."""
    pipeline = dlt.pipeline(
        pipeline_name="test_missing_collection_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        # Try to access non-existent collection
        result = client._request("GET", "/collections/non_existent_collection_12345")
        assert result is None


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_empty_batch() -> None:
    """Test that empty batches are handled correctly."""

    @dlt.resource
    def empty_data():
        # Yield nothing
        return
        yield

    pipeline = dlt.pipeline(
        pipeline_name="test_empty_batch_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    # This should complete without error, even with no data
    info = pipeline.run(empty_data())
    assert_load_info(info)

    # Collection might not be created if no data, which is fine
    # The important thing is it doesn't crash


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_special_characters() -> None:
    """Test that special characters and Unicode are handled correctly."""

    @dlt.resource
    def special_data():
        yield [
            {
                "id": "1",
                "name": "José García",
                "description": "Test with émojis 🎉 and spéciál chárs",
                "unicode": "测试中文",
            }
        ]

    pipeline = dlt.pipeline(
        pipeline_name="test_special_chars_" + uniq_id(),
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_dataset_" + uniq_id(),
    )

    info = pipeline.run(special_data())
    assert_load_info(info)

    # Verify data is in Typesense with special characters preserved
    assert_collection(
        pipeline,
        "special_data",
        expected_items_count=1,
        items=[
            {
                "id": "1",
                "name": "José García",
                "description": "Test with émojis 🎉 and spéciál chárs",
                "unicode": "测试中文",
            }
        ],
    )
