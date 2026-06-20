import typing as t

from dlt.common.destination import (
    Destination,
    DestinationCapabilitiesContext,
    PreparedTableSchema,
)
from dlt.common.schema.typing import TColumnSchema
from dlt.destinations.type_mapping import TypeMapperImpl

from dlt_typesense.configuration import TypesenseClientConfiguration

if t.TYPE_CHECKING:
    from dlt_typesense.typesense_client import TypesenseClient


class TypesenseTypeMapper(TypeMapperImpl):
    sct_to_unbound_dbt = {
        "text": "string",
        "double": "float",
        "bool": "bool",
        "timestamp": "int64",
        "date": "int64",
        "time": "string",
        "bigint": "int64",
        "binary": "string",
        "decimal": "string",
        "wei": "string",
        "json": "object",
        "complex": "object",
    }

    sct_to_dbt = {}

    dbt_to_sct = {
        "string": "text",
        "float": "double",
        "bool": "bool",
        "int64": "bigint",
        "int32": "bigint",
        "object": "json",
    }

    def to_destination_type(
        self, column: TColumnSchema, table: PreparedTableSchema
    ) -> str:
        data_type = column["data_type"]
        if data_type == "json" and column.get("x-typesense-facet", False):
            return "string[]"
        if data_type.endswith("[]"):
            base_type = data_type[:-2]
            if base_type in self.sct_to_unbound_dbt:
                return f"{self.sct_to_unbound_dbt[base_type]}[]"
            if base_type in self.sct_to_dbt:
                return f"{self.sct_to_dbt[base_type]}[]"
        return super().to_destination_type(column, table)


class typesense(Destination[TypesenseClientConfiguration, "TypesenseClient"]):
    spec = TypesenseClientConfiguration

    def _raw_capabilities(self) -> DestinationCapabilitiesContext:
        caps = DestinationCapabilitiesContext()
        caps.preferred_loader_file_format = "jsonl"
        caps.supported_loader_file_formats = ["jsonl"]
        caps.has_case_sensitive_identifiers = True
        caps.max_identifier_length = 255
        caps.max_column_identifier_length = 255
        caps.supports_ddl_transactions = False
        caps.supported_replace_strategies = ["truncate-and-insert"]
        caps.supported_merge_strategies = ["upsert"]
        caps.type_mapper = TypesenseTypeMapper
        return caps

    @property
    def client_class(self) -> t.Type["TypesenseClient"]:
        from dlt_typesense.typesense_client import TypesenseClient

        return TypesenseClient


typesense.register()
