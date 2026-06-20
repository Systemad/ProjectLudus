from typing import Any, Dict, List, Literal, Union

from dlt.common.schema.typing import TTableSchemaColumns
from dlt.extract import DltResource
from dlt.destinations.utils import get_resource_for_adapter

TColumnNames = Union[str, List[str]]


def typesense_adapter(
    data: Any,
    facet: TColumnNames = None,
    index: TColumnNames = None,
    sort: TColumnNames = None,
    # auto_schema: bool = None, # Table level hint?
) -> DltResource:
    """Prepares data for Typesense destination by applying column hints.

    Args:
        data (Any): The data to be transformed. It can be raw data or an instance
            of DltResource.
        facet (TColumnNames, optional): Columns to enable faceting for.
        index (TColumnNames, optional): Columns to enable/disable indexing for.
            Currently only supports enabling (which is default).
            To disable indexing, Typesense schema needs 'index': False.
            Here we assume if you pass a column to index, you want to index it.
            Wait, index=False?
            Let's allow TColumnNames for now, assuming True.
        sort (TColumnNames, optional): Columns to enable sorting for.

    Returns:
        DltResource: A resource with applied Typesense-specific hints.
    """
    resource = get_resource_for_adapter(data)
    column_hints: TTableSchemaColumns = {}

    def _apply_hint(columns: TColumnNames, hint: str, value: Any) -> None:
        if columns:
            if isinstance(columns, str):
                columns = [columns]
            for col in columns:
                if col not in column_hints:
                    column_hints[col] = {"name": col}
                column_hints[col][hint] = value  # type: ignore

    _apply_hint(facet, "x-typesense-facet", True)
    _apply_hint(index, "x-typesense-index", True)  # What if we want False?
    _apply_hint(sort, "x-typesense-sort", True)

    if column_hints:
        resource.apply_hints(columns=column_hints)

    return resource
