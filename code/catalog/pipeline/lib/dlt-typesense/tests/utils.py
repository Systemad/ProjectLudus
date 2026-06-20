import os
import dlt
from typing import Any, List, Optional, Dict

from dlt.common.pipeline import PipelineContext
from dlt.common.configuration.container import Container
from dlt.common.schema import Schema
from dlt.common.utils import uniq_id
from dlt_typesense.factory import typesense
from dlt_typesense.typesense_client import TypesenseClient

# Test storage root directory
TEST_STORAGE_ROOT = os.path.join(os.path.dirname(__file__), ".test_storage")


def assert_unordered_list_equal(list1: List[Any], list2: List[Any]) -> None:
    """Assert two lists are equal ignoring order."""
    assert len(list1) == len(
        list2
    ), f"Lists have different length: {len(list1)} vs {len(list2)}"
    for item in list1:
        assert item in list2, f"Item {item} not found in list2"


def assert_collection(
    pipeline: dlt.Pipeline,
    collection_name: str,
    expected_items_count: Optional[int] = None,
    items: Optional[List[Any]] = None,
) -> None:
    """Verify collection exists and contains expected data."""
    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        qualified_name = client.make_qualified_collection_name(collection_name)

        # Check collection exists
        collection = client._request("GET", f"/collections/{qualified_name}")
        assert collection is not None, f"Collection {qualified_name} does not exist"

        # Search all documents
        search_params = {"q": "*", "per_page": 250}  # Adjust as needed
        results = client._request(
            "GET",
            f"/collections/{qualified_name}/documents/search",
            params=search_params,
        )

        if not results:
            documents = []
        else:
            hits = results.get("hits", [])
            documents = [hit["document"] for hit in hits]

        if expected_items_count is not None:
            assert (
                len(documents) == expected_items_count
            ), f"Expected {expected_items_count} documents, got {len(documents)}"

        if items is not None:
            # Remove dlt internal fields for comparison
            drop_keys = ["_dlt_id", "_dlt_load_id"]
            cleaned_docs = [
                {k: v for k, v in doc.items() if k not in drop_keys}
                for doc in documents
            ]

            # Compare (order-independent)
            assert_unordered_list_equal(cleaned_docs, items)


def drop_active_pipeline_data() -> None:
    """Clean up test data after tests."""
    from dlt.common.pipeline import PipelineContext
    from dlt.common.configuration.container import Container

    if Container()[PipelineContext].is_active():
        p = dlt.pipeline()
        client: TypesenseClient

        with p.destination_client() as client:  # type: ignore[assignment]
            client.drop_storage()

        Container()[PipelineContext].deactivate()


def get_typesense_client(
    schema: Schema = None, dataset_name: str = None
) -> TypesenseClient:
    """Create a TypesenseClient instance for testing."""
    if schema is None:
        schema = Schema("test_schema")
    if dataset_name is None:
        dataset_name = "ClientTest" + uniq_id()
    dest = typesense()
    config = dest.spec()._bind_dataset_name(dataset_name=dataset_name)
    return dest.client(schema, config)


def get_collection_schema(
    client: TypesenseClient, collection_name: str
) -> Optional[Dict[str, Any]]:
    """Query Typesense to get the actual collection schema."""
    qualified_name = client.make_qualified_collection_name(collection_name)
    collection = client._request("GET", f"/collections/{qualified_name}")
    if collection:
        return collection.get("fields", [])
    return None


def assert_collection_schema(
    pipeline: dlt.Pipeline,
    collection_name: str,
    expected_fields: Optional[List[Dict[str, Any]]] = None,
) -> None:
    """Verify Typesense collection schema matches expected fields."""
    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        actual_fields = get_collection_schema(client, collection_name)
        assert actual_fields is not None, f"Collection {collection_name} does not exist"

        if expected_fields is not None:
            # Create a map of field names to field definitions for easier comparison
            actual_field_map = {f["name"]: f for f in actual_fields}
            expected_field_map = {f["name"]: f for f in expected_fields}

            # Check all expected fields exist
            for field_name, expected_field in expected_field_map.items():
                assert (
                    field_name in actual_field_map
                ), f"Field {field_name} not found in collection"
                actual_field = actual_field_map[field_name]

                # Verify field type
                if "type" in expected_field:
                    assert actual_field.get("type") == expected_field["type"], (
                        f"Field {field_name} type mismatch: expected {expected_field['type']}, got"
                        f" {actual_field.get('type')}"
                    )

                # Verify field properties
                for prop in ["facet", "sort", "index", "optional"]:
                    if prop in expected_field:
                        assert actual_field.get(prop) == expected_field[prop], (
                            f"Field {field_name} {prop} mismatch: expected {expected_field[prop]},"
                            f" got {actual_field.get(prop)}"
                        )
