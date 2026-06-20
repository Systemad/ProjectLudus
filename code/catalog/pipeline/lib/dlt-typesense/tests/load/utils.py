"""Load job testing utilities."""

import os
from typing import Any, Dict, List, Optional
from io import BytesIO

from dlt.common import json
from dlt.common.storages import FileStorage
from dlt.common.schema.utils import TTableSchema
from dlt_typesense.typesense_client import TypesenseClient


def write_dataset(
    client: TypesenseClient,
    file_obj: BytesIO,
    data: List[Dict[str, Any]],
    table_schema: TTableSchema,
) -> None:
    """Write test data to a file in JSONL format.

    Args:
        client: TypesenseClient instance (unused, kept for API consistency)
        file_obj: File-like object to write to (BytesIO)
        data: List of dictionaries representing rows
        table_schema: Table schema definition (unused, kept for API consistency)
    """
    for row in data:
        json_line = json.dumps(row)
        file_obj.write(json_line.encode("utf-8"))
        file_obj.write(b"\n")


def expect_load_file(
    client: TypesenseClient,
    file_storage: FileStorage,
    file_content: bytes,
    table_name: str,
    status: str = "completed",
) -> None:
    """Create a load job from file content and execute it.

    Args:
        client: TypesenseClient instance
        file_storage: FileStorage instance for temporary file storage
        file_content: Bytes content of the JSONL file
        table_name: Name of the table/collection to load into
        status: Expected job status (default: "completed")

    Raises:
        AssertionError: If load job doesn't complete successfully
    """
    from dlt.common.schema.utils import new_table
    from dlt.common.destination.client import PreparedTableSchema

    # Get table schema from client
    table = client.schema.get_table(table_name)
    if not table:
        # Create a basic table schema if it doesn't exist
        table = new_table(table_name, write_disposition="append")
        client.schema.update_table(table)

    # Create prepared table schema
    prepared_table: PreparedTableSchema = {
        "name": table_name,
        "columns": table.get("columns", {}),
        "write_disposition": table.get("write_disposition", "append"),
    }

    # Save file content to temporary file
    # dlt expects filenames in format: table_name.file_id.retry_count.file_format
    # Format: table_name.file_id.retry_count.file_format (e.g., "users.test_load_id.0.jsonl")
    load_id = "test_load_id"
    retry_count = 0
    file_format = "jsonl"
    filename = f"{table_name}.{load_id}.{retry_count}.{file_format}"
    file_path = file_storage.save(filename, file_content)

    try:
        # Create and run load job
        load_job = client.create_load_job(prepared_table, file_path, load_id)
        load_job.run()

        # Verify job completed
        if status == "completed":
            assert (
                load_job.exception() is None
            ), f"Load job failed: {load_job.exception()}"
    finally:
        # Clean up temporary file
        if file_storage.has_file(file_path):
            file_storage.delete(file_path)
