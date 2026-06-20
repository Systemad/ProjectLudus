"""Tests for Typesense configuration defaults and custom values."""

import os
import pytest
from dlt.common.schema import Schema
from dlt_typesense.configuration import (
    TypesenseCredentials,
    TypesenseClientConfiguration,
    DEFAULT_CONNECTION_TIMEOUT_SECONDS,
    DEFAULT_BATCH_SIZE,
    DEFAULT_IMPORT_BATCH_SIZE,
    DEFAULT_DATASET_SEPARATOR,
)
from dlt_typesense.factory import typesense
from dlt_typesense.typesense_client import TypesenseClient


def test_default_constants_defined() -> None:
    """Test that all default constants are properly defined."""
    assert DEFAULT_CONNECTION_TIMEOUT_SECONDS == 10.0
    assert DEFAULT_BATCH_SIZE == 1000
    assert DEFAULT_IMPORT_BATCH_SIZE == 40
    assert DEFAULT_DATASET_SEPARATOR == "___"


def test_credentials_default_timeout() -> None:
    """Test that TypesenseCredentials uses default timeout when not specified."""
    # This will fail validation because url and api_key are required
    # but we can check the default value before validation
    creds = TypesenseCredentials()
    assert creds.connection_timeout_seconds == DEFAULT_CONNECTION_TIMEOUT_SECONDS


def test_credentials_custom_timeout() -> None:
    """Test that custom timeout can be set in TypesenseCredentials."""
    custom_timeout = 30.0
    creds = TypesenseCredentials(connection_timeout_seconds=custom_timeout)
    assert creds.connection_timeout_seconds == custom_timeout


def test_client_config_default_values() -> None:
    """Test that TypesenseClientConfiguration uses default values when not specified."""
    config = TypesenseClientConfiguration()
    assert config.batch_size == DEFAULT_BATCH_SIZE
    assert config.import_batch_size == DEFAULT_IMPORT_BATCH_SIZE
    assert config.dataset_separator == DEFAULT_DATASET_SEPARATOR


def test_client_config_custom_values() -> None:
    """Test that custom values can be set in TypesenseClientConfiguration."""
    custom_batch_size = 2000
    custom_import_batch_size = 50
    custom_separator = "|||"

    config = TypesenseClientConfiguration(
        batch_size=custom_batch_size,
        import_batch_size=custom_import_batch_size,
        dataset_separator=custom_separator,
    )
    assert config.batch_size == custom_batch_size
    assert config.import_batch_size == custom_import_batch_size
    assert config.dataset_separator == custom_separator


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_client_uses_custom_timeout() -> None:
    """Test that TypesenseClient uses custom connection timeout."""
    from dlt.common.utils import uniq_id

    custom_timeout = 15.0
    schema = Schema("test_schema")
    dest = typesense()
    dataset_name = "TestTimeout" + uniq_id()
    config = dest.spec()._bind_dataset_name(dataset_name=dataset_name)

    # Initialize credentials if None (will be populated from env vars when resolved)
    if config.credentials is None:
        config.credentials = TypesenseCredentials()

    # Set custom timeout
    config.credentials.connection_timeout_seconds = custom_timeout

    client = dest.client(schema, config)
    assert client.timeout == custom_timeout


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_client_uses_default_timeout() -> None:
    """Test that TypesenseClient uses default connection timeout when not specified."""
    from dlt.common.utils import uniq_id

    schema = Schema("test_schema")
    dest = typesense()
    dataset_name = "TestDefaultTimeout" + uniq_id()
    config = dest.spec()._bind_dataset_name(dataset_name=dataset_name)

    client = dest.client(schema, config)
    assert client.timeout == DEFAULT_CONNECTION_TIMEOUT_SECONDS


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_dataset_separator_in_collection_name(typesense_client: TypesenseClient) -> None:
    """Test that custom dataset_separator is used in collection name generation."""
    custom_separator = "|||"
    typesense_client.config.dataset_separator = custom_separator

    table_name = "test_table"
    qualified_name = typesense_client.make_qualified_collection_name(table_name)

    expected_name = f"{typesense_client.dataset_name}{custom_separator}{table_name}"
    assert qualified_name == expected_name
    assert custom_separator in qualified_name


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_default_dataset_separator_in_collection_name(
    typesense_client: TypesenseClient,
) -> None:
    """Test that default dataset_separator is used in collection name generation."""
    table_name = "test_table"
    qualified_name = typesense_client.make_qualified_collection_name(table_name)

    expected_name = (
        f"{typesense_client.dataset_name}{DEFAULT_DATASET_SEPARATOR}{table_name}"
    )
    assert qualified_name == expected_name
    assert DEFAULT_DATASET_SEPARATOR in qualified_name


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_import_batch_size_affects_batching(
    typesense_client: TypesenseClient,
) -> None:
    """Test that import_batch_size configuration is accessible and can be modified."""
    # Verify default value
    assert typesense_client.config.import_batch_size == DEFAULT_IMPORT_BATCH_SIZE

    # Set custom value
    custom_batch_size = 5
    typesense_client.config.import_batch_size = custom_batch_size
    assert typesense_client.config.import_batch_size == custom_batch_size


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_batch_size_accessible(typesense_client: TypesenseClient) -> None:
    """Test that batch_size configuration is accessible."""
    # Verify default value
    assert typesense_client.config.batch_size == DEFAULT_BATCH_SIZE

    # Set custom value
    custom_batch_size = 2000
    typesense_client.config.batch_size = custom_batch_size
    assert typesense_client.config.batch_size == custom_batch_size

