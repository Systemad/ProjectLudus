import os
import pytest
from typing import Iterator

import dlt
from dlt.common.schema import Schema
from dlt.common.storages import FileStorage
from dlt.common.utils import uniq_id

# Import the main module to ensure destination registration
import dlt_typesense  # noqa: F401
from dlt_typesense.factory import typesense
from dlt_typesense.typesense_client import TypesenseClient
from tests.utils import drop_active_pipeline_data
from tests.utils import TEST_STORAGE_ROOT


@pytest.fixture(autouse=True)
def drop_typesense_data() -> Iterator[None]:
    """Auto-cleanup after each test."""
    yield
    drop_active_pipeline_data()


def get_typesense_client_instance(
    schema: Schema, dataset_name: str = None
) -> TypesenseClient:
    """Create a TypesenseClient instance for testing."""
    dest = typesense()
    if dataset_name is None:
        dataset_name = "ClientTest" + uniq_id()
    config = dest.spec()._bind_dataset_name(dataset_name=dataset_name)
    return dest.client(schema, config)


@pytest.fixture(scope="function")
def typesense_client() -> Iterator[TypesenseClient]:
    """Fixture that provides a TypesenseClient instance with automatic cleanup."""
    schema = Schema("test_schema")
    client = get_typesense_client_instance(schema)
    with client:
        try:
            yield client
        finally:
            # Cleanup: drop storage and close session
            try:
                if client._session is not None:
                    client.drop_storage()
            finally:
                if client._session is not None:
                    client._session.close()
                    client._session = None


@pytest.fixture(scope="function")
def typesense_pipeline() -> Iterator[dlt.Pipeline]:
    """Fixture that provides a dlt Pipeline instance with automatic cleanup."""
    pipeline_name = "test_pipeline_" + uniq_id()
    dataset_name = "test_dataset_" + uniq_id()

    pipeline = dlt.pipeline(
        pipeline_name=pipeline_name,
        destination="dlt_typesense.factory.typesense",
        dataset_name=dataset_name,
    )

    try:
        yield pipeline
    finally:
        # Cleanup is handled by drop_typesense_data fixture
        pass


@pytest.fixture
def file_storage() -> FileStorage:
    """Fixture for file storage operations."""
    return FileStorage(TEST_STORAGE_ROOT, file_type="b", makedirs=True)


@pytest.fixture
def skip_if_no_typesense() -> None:
    """Skip test if Typesense credentials are not configured."""
    if not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"):
        pytest.skip("Typesense credentials not configured")
