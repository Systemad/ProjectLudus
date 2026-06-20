"""Test data generation utilities for dlt data types."""

from typing import Dict, Any, Tuple


def table_update_and_row() -> Tuple[Dict[str, Dict[str, Any]], Dict[str, Any]]:
    """Generate test table schemas and sample data for all dlt data types.

    Returns:
        Tuple of (column_schemas dict, data_types dict) where:
        - column_schemas: dict mapping column names to column schema definitions
        - data_types: dict mapping column names to sample data values
    """
    from datetime import datetime

    column_schemas: Dict[str, Dict[str, Any]] = {
        "col1": {"name": "col1", "data_type": "text"},
        "col2": {"name": "col2", "data_type": "double"},
        "col3": {"name": "col3", "data_type": "bool"},
        "col4": {"name": "col4", "data_type": "timestamp"},
        "col5": {"name": "col5", "data_type": "date"},
        "col6": {"name": "col6", "data_type": "time"},
        "col7": {"name": "col7", "data_type": "bigint"},
        "col8": {"name": "col8", "data_type": "binary"},
        "col9": {"name": "col9", "data_type": "decimal"},
        "col10": {"name": "col10", "data_type": "wei"},
        "col11": {"name": "col11", "data_type": "json"},
        "col12": {"name": "col12", "data_type": "timestamp"},  # timestamp with timezone
        "col13": {"name": "col13", "data_type": "complex"},
    }

    data_types: Dict[str, Any] = {
        "col1": "test_string",
        "col2": 3.14,
        "col3": True,
        "col4": datetime(2023, 1, 1, 12, 0, 0),
        "col5": datetime(2023, 1, 1).date(),
        "col6": "12:00:00",
        "col7": 123456789012345,
        "col8": b"binary_data",
        "col9": "123.456",
        "col10": "1000000000000000000",
        "col11": {"key": "value", "nested": {"foo": "bar"}},
        "col12": datetime(2023, 1, 1, 12, 0, 0),
        "col13": {"complex": "object", "with": ["nested", "data"]},
    }

    return column_schemas, data_types
