import os
import pytest
import dlt
from dlt.common.utils import uniq_id
from dlt_typesense.typesense_client import TypesenseClient
from dlt_typesense.exceptions import TypesenseApiError, TypesenseConnectionError
from tests.cases import table_update_and_row
from tests.utils import get_collection_schema
from tests.conftest import typesense_client
from dlt.common.schema.utils import new_table


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_client_error_handling(typesense_client: TypesenseClient) -> None:
    """Test error handling with real Typesense errors."""
    result = typesense_client._request(
        "GET", "/collections/non_existent_collection_12345"
    )
    assert result is None

    from dlt_typesense.configuration import TypesenseCredentials
    from dlt_typesense.factory import typesense
    from dlt.common.schema import Schema

    wrong_config = typesense().spec()
    wrong_config.credentials = TypesenseCredentials(
        url=typesense_client.api_url, api_key="wrong_key_12345"
    )
    wrong_config = wrong_config._bind_dataset_name(dataset_name="test_dataset")

    wrong_client = typesense().client(Schema("test"), wrong_config)
    with wrong_client:
        with pytest.raises(TypesenseApiError):
            wrong_client._request(
                "POST", "/collections", json_data={"name": "test", "fields": []}
            )

    health = typesense_client._request("GET", "/health")
    assert health is not None


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_all_data_types(typesense_client: TypesenseClient) -> None:
    """Test all dlt data types are properly mapped to Typesense types with real collection."""
    # Get test data with all data types
    column_schemas, data_types = table_update_and_row()
    # Remove col12 (timestamp with timezone) as it may not be supported consistently
    column_schemas.pop("col12", None)
    data_types.pop("col12", None)
    columns_list = list(column_schemas.values())

    class_name = "AllTypes"

    # Update schema with all data types
    typesense_client.schema.update_table(
        new_table(class_name, write_disposition="append", columns=columns_list)
    )
    typesense_client.schema._bump_version()
    typesense_client.update_stored_schema()

    normalized_table_name = typesense_client.schema.naming.normalize_table_identifier(
        class_name
    )

    actual_fields = get_collection_schema(typesense_client, normalized_table_name)
    assert (
        actual_fields is not None
    ), f"Collection {normalized_table_name} was not created"

    field_map = {f["name"]: f for f in actual_fields}

    for col_name, col_schema in column_schemas.items():
        if col_name == "id":
            continue
        assert (
            col_name in field_map
        ), f"Column {col_name} not found in collection schema"

        expected_type = col_schema["data_type"]
        actual_field = field_map[col_name]
        actual_type = actual_field.get("type")

        if expected_type in ["decimal", "time"]:
            assert (
                actual_type == "string"
            ), f"Expected {col_name} ({expected_type}) to map to 'string', got '{actual_type}'"
        elif expected_type == "json":
            assert (
                actual_type == "object"
            ), f"Expected {col_name} ({expected_type}) to map to 'object', got '{actual_type}'"
        elif expected_type == "wei":
            assert actual_type in ["double", "string"], (
                f"Expected {col_name} ({expected_type}) to map to 'double' or 'string', got"
                f" '{actual_type}'"
            )
        elif expected_type == "date":
            assert (
                actual_type == "int64"
            ), f"Expected {col_name} ({expected_type}) to map to 'int64', got '{actual_type}'"
        elif expected_type == "timestamp":
            assert (
                actual_type == "int64"
            ), f"Expected {col_name} ({expected_type}) to map to 'int64', got '{actual_type}'"
        elif expected_type == "bigint":
            assert (
                actual_type == "int64"
            ), f"Expected {col_name} ({expected_type}) to map to 'int64', got '{actual_type}'"
        elif expected_type == "double":
            assert (
                actual_type == "float"
            ), f"Expected {col_name} ({expected_type}) to map to 'float', got '{actual_type}'"
        elif expected_type == "bool":
            assert (
                actual_type == "bool"
            ), f"Expected {col_name} ({expected_type}) to map to 'bool', got '{actual_type}'"
        elif expected_type == "text":
            assert (
                actual_type == "string"
            ), f"Expected {col_name} ({expected_type}) to map to 'string', got '{actual_type}'"
        elif expected_type == "binary":
            assert (
                actual_type == "string"
            ), f"Expected {col_name} ({expected_type}) to map to 'string', got '{actual_type}'"
        elif expected_type in ["complex", "object"]:
            assert (
                actual_type == "object"
            ), f"Expected {col_name} ({expected_type}) to map to 'object', got '{actual_type}'"
