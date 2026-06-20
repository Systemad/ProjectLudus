import typing as t
import requests

from dlt.common import logger, json, pendulum
from dlt.common.destination import DestinationCapabilitiesContext
from dlt.common.destination.client import (
    JobClientBase,
    WithStateSync,
    StorageSchemaInfo,
    StateInfo,
    RunnableLoadJob,
    LoadJob,
    PreparedTableSchema,
)
from dlt.common.schema import Schema, TSchemaTables
from dlt.common.schema.utils import (
    get_columns_names_with_prop,
    version_table,
    loads_table,
    normalize_table_identifiers,
)
from dlt.common.schema.typing import C_DLT_LOAD_ID, C_DLT_LOADS_TABLE_LOAD_ID
from dlt.common.storages import FileStorage
from dlt.common.time import ensure_pendulum_datetime_utc
from dlt.common.utils import digest128
from dlt_typesense.configuration import TypesenseClientConfiguration
from dlt_typesense.exceptions import (
    TypesenseConnectionError,
    TypesenseApiError,
    TypesenseSchemaError,
    TypesenseBulkImportError,
)
from dlt_typesense.typing import TTypesenseAction
from dlt.destinations.utils import get_pipeline_state_query_columns

if t.TYPE_CHECKING:
    pass


class LoadTypesenseJob(RunnableLoadJob):
    def __init__(
        self,
        file_path: str,
        table: "PreparedTableSchema",
        client: "TypesenseClient",
    ) -> None:
        super().__init__(file_path)
        self._table = table
        self._client = client
        self._config = client.config
        # Determine action mode
        write_disposition = table.get("write_disposition", "append")
        self._action: TTypesenseAction = "create"
        if write_disposition == "replace":
            self._action = "create"  # We assume table was truncated
        elif write_disposition == "merge":
            self._action = "upsert"
        # Override from config if specified
        # self._action = self._config.import_action or self._action

    def run(self) -> None:
        with self._client:
            qualified_name = self._client.make_qualified_collection_name(
                self._table["name"]
            )

            table_columns = self._table.get("columns", {})
            json_cols = [
                n
                for n, c in table_columns.items()
                if c["data_type"] in ("json", "complex")
            ]
            date_cols = [
                n
                for n, c in table_columns.items()
                if c["data_type"] in ("date", "timestamp")
            ]

            primary_keys = get_columns_names_with_prop(self._table, "primary_key")

            batch = []
            batch_size = self._config.import_batch_size

            with FileStorage.open_zipsafe_ro(self._file_path) as f:
                for line in f:
                    row = json.loads(line)

                    for col in json_cols:
                        if col in row and row[col] is not None:
                            pass

                    for col in date_cols:
                        if col in row and row[col] is not None:
                            dt = ensure_pendulum_datetime_utc(row[col])
                            row[col] = int(dt.timestamp())

                    if "id" not in row:
                        if primary_keys:
                            key_values = [str(row.get(k)) for k in primary_keys]
                            row["id"] = digest128("_".join(key_values))
                    if "id" in row and row["id"] is not None:
                        row["id"] = str(row["id"])

                    batch.append(row)

                    if len(batch) >= batch_size:
                        self._flush_batch(qualified_name, batch)
                        batch = []

                if batch:
                    self._flush_batch(qualified_name, batch)

    def _flush_batch(
        self, collection_name: str, batch: t.List[t.Dict[str, t.Any]]
    ) -> None:
        if not batch:
            return

        jsonl_data = "\n".join([json.dumps(doc) for doc in batch])

        response = self._client.import_documents(
            collection_name, jsonl_data, self._action
        )

        # Parse response (one JSON per line)
        success = True
        errors = []
        for line in response.splitlines():
            try:
                res = json.loads(line)
                if not res.get("success"):
                    success = False
                    errors.append(res.get("error", "Unknown error"))
            except Exception:
                success = False
                errors.append(f"Failed to parse response: {line}")

        if not success:
            raise TypesenseBulkImportError(f"Bulk import failed with errors: {errors}")


class TypesenseClient(JobClientBase, WithStateSync):
    """Typesense destination handler."""

    def __init__(
        self,
        schema: Schema,
        config: TypesenseClientConfiguration,
        capabilities: DestinationCapabilitiesContext,
    ) -> None:
        super().__init__(schema, config, capabilities)
        self.config: TypesenseClientConfiguration = config
        self.type_mapper = self.capabilities.get_type_mapper()
        self.api_url = self.config.credentials.url
        self.api_key = self.config.credentials.api_key
        self.timeout = self.config.credentials.connection_timeout_seconds
        self._session: t.Optional[requests.Session] = None

        # Prepare state table schemas
        version_table_ = normalize_table_identifiers(version_table(), schema.naming)
        self.version_collection_properties = list(version_table_["columns"].keys())
        loads_table_ = normalize_table_identifiers(loads_table(), schema.naming)
        self.loads_collection_properties = list(loads_table_["columns"].keys())
        state_table_ = normalize_table_identifiers(
            get_pipeline_state_query_columns(), schema.naming
        )
        self.pipeline_state_properties = list(state_table_["columns"].keys())

    @property
    def dataset_name(self) -> str:
        return self.config.normalize_dataset_name(self.schema)

    @property
    def sentinel_collection(self) -> str:
        return self.dataset_name or "DltSentinelCollection"

    def make_qualified_collection_name(self, table_name: str) -> str:
        dataset_separator = self.config.dataset_separator
        return (
            f"{self.dataset_name}{dataset_separator}{table_name}"
            if self.dataset_name
            else table_name
        )

    def _request(
        self,
        method: str,
        path: str,
        json_data: t.Any = None,
        data: t.Any = None,
        params: t.Dict[str, t.Any] = None,
    ) -> t.Any:
        if self._session is None:
            raise TypesenseConnectionError("Client is not connected")

        url = f"{self.api_url}{path}"
        try:
            response = self._session.request(
                method,
                url,
                json=json_data,
                data=data,
                params=params,
                timeout=self.timeout,
            )

            if response.status_code >= 500:
                raise TypesenseConnectionError(
                    f"Server error {response.status_code}: {response.text}"
                )

            if response.status_code == 404:
                return None

            if response.status_code >= 400:
                try:
                    error_msg = response.json().get("message", response.text)
                except Exception:
                    error_msg = response.text

                if response.status_code == 401 or response.status_code == 403:
                    raise TypesenseApiError(
                        f"Authentication error {response.status_code}: {error_msg}"
                    )

                raise TypesenseApiError(
                    f"API error {response.status_code}: {error_msg}"
                )

            try:
                # If content type is jsonl or text, return text, else json
                # But Typesense import returns JSONL string response.
                if path.endswith("/import"):
                    return response.text
                return response.json()
            except Exception:
                return response.text

        except requests.exceptions.ConnectionError as e:
            raise TypesenseConnectionError(f"Connection failed: {e}")
        except requests.exceptions.Timeout as e:
            raise TypesenseConnectionError(f"Request timed out: {e}")

    def initialize_storage(self, truncate_tables: t.Iterable[str] = None) -> None:
        if not self.is_storage_initialized():
            self._create_sentinel_collection()
        elif truncate_tables:
            for table_name in truncate_tables:
                qualified_name = self.make_qualified_collection_name(table_name)
                existing = self._request("GET", f"/collections/{qualified_name}")
                if existing:
                    # Use Typesense's truncate API to remove all documents while keeping collection and schema
                    # DELETE /collections/:collection/documents?truncate=true
                    try:
                        result = self._request(
                            "DELETE",
                            f"/collections/{qualified_name}/documents",
                            params={"truncate": "true"},
                        )
                        # Verify truncate succeeded (should return {"num_deleted": N})
                        if result:
                            if isinstance(result, dict) and "num_deleted" in result:
                                logger.info(
                                    f"Truncated {result['num_deleted']} documents from"
                                    f" {qualified_name}"
                                )
                            else:
                                logger.warning(
                                    f"Truncate response for {qualified_name} was unexpected:"
                                    f" {result}"
                                )
                    except TypesenseApiError as e:
                        # Log but don't fail - collection might not exist or already empty
                        logger.warning(
                            f"Failed to truncate collection {qualified_name}: {e}"
                        )
                        pass

    def is_storage_initialized(self) -> bool:
        return (
            self._request("GET", f"/collections/{self.sentinel_collection}") is not None
        )

    def drop_storage(self) -> None:
        collections = self._request("GET", "/collections")
        if not collections:
            return

        prefix = (
            f"{self.dataset_name}{self.config.dataset_separator}"
            if self.dataset_name
            else ""
        )

        for collection in collections:
            name = collection["name"]
            if (prefix and name.startswith(prefix)) or name == self.sentinel_collection:
                self._request("DELETE", f"/collections/{name}")

        if not self.dataset_name:
            # if no dataset name, delete schema tables explicitly if they exist
            for table_name in self.schema.tables.keys():
                self._request("DELETE", f"/collections/{table_name}")

    def update_stored_schema(
        self,
        only_tables: t.Iterable[str] = None,
        expected_update: TSchemaTables = None,
    ) -> t.Optional[TSchemaTables]:
        applied_update = super().update_stored_schema(only_tables, expected_update)

        tables_to_update = only_tables or self.schema.tables.keys()
        for table_name in tables_to_update:
            qualified_name = self.make_qualified_collection_name(table_name)
            existing = self._request("GET", f"/collections/{qualified_name}")

            if not existing:
                self._create_collection(table_name)
            else:
                self._update_collection_schema(table_name)

        version_table_name = self.schema.version_table_name
        version_qualified_name = self.make_qualified_collection_name(version_table_name)
        version_exists = self._request("GET", f"/collections/{version_qualified_name}")
        if not version_exists:
            self._create_collection(version_table_name)

        self._update_schema_in_storage(self.schema)
        return applied_update

    def _create_collection(self, table_name: str) -> None:
        qualified_name = self.make_qualified_collection_name(table_name)
        table = self.schema.get_table(table_name)

        fields = []
        has_object_type = False
        for col_name, col in table["columns"].items():
            if col_name == "id":
                continue
            if "data_type" not in col:
                continue
            field_schema = self._make_field_schema(col)
            fields.append(field_schema)
            if field_schema.get("type") == "object":
                has_object_type = True

        schema = {
            "name": qualified_name,
            "fields": fields,
        }

        if has_object_type:
            schema["enable_nested_fields"] = True

        self._request("POST", "/collections", json_data=schema)

    def _update_collection_schema(self, table_name: str) -> None:
        qualified_name = self.make_qualified_collection_name(table_name)
        table = self.schema.get_table(table_name)

        current_schema = self._request("GET", f"/collections/{qualified_name}")
        current_fields = {f["name"] for f in current_schema.get("fields", [])}

        new_fields = []
        for col_name, col in table["columns"].items():
            # Skip 'id' field - Typesense handles it automatically and doesn't allow modification
            if col_name == "id":
                continue
            # Skip columns without data_type (incomplete columns that haven't received data yet)
            if "data_type" not in col:
                continue
            if col_name not in current_fields:
                new_fields.append(self._make_field_schema(col))

        if new_fields:
            self._request(
                "PATCH",
                f"/collections/{qualified_name}",
                json_data={"fields": new_fields},
            )

    def _make_field_schema(self, column: t.Any) -> t.Dict[str, t.Any]:
        field_type = self.type_mapper.to_destination_type(column, None)
        # For text fields in version/state tables, make them facetable so filter_by works
        # Typesense requires fields to be facetable for filter_by to work on them
        is_facetable = column.get("x-typesense-facet", False)
        # Auto-enable facet for text fields in dlt internal tables if not explicitly set
        if field_type == "string" and not is_facetable:
            # Check if this is a dlt internal table field that might need filtering
            # (schema_name, pipeline_name, etc.)
            if column["name"] in ("schema_name", "pipeline_name", "version_hash"):
                is_facetable = True

        return {
            "name": column["name"],
            "type": field_type,
            "optional": column.get("nullable", True),
            "facet": is_facetable,
            "index": column.get("x-typesense-index", True),
            "sort": column.get("x-typesense-sort", False),
        }

    def get_stored_state(self, pipeline_name: str) -> t.Optional[StateInfo]:
        p_load_id = self.schema.naming.normalize_identifier(C_DLT_LOADS_TABLE_LOAD_ID)
        p_dlt_load_id = self.schema.naming.normalize_identifier(C_DLT_LOAD_ID)
        p_pipeline_name = self.schema.naming.normalize_identifier("pipeline_name")
        p_created_at = self.schema.naming.normalize_identifier("created_at")

        state_collection = self.make_qualified_collection_name(
            self.schema.state_table_name
        )
        loads_collection = self.make_qualified_collection_name(
            self.schema.loads_table_name
        )

        # Fetch all state documents and filter in Python (more reliable than filter_by)
        search_params = {"q": "*", "per_page": 100}  # Get enough to find the right one

        try:
            results = self._request(
                "GET",
                f"/collections/{state_collection}/documents/search",
                params=search_params,
            )
        except TypesenseApiError:
            return None  # Collection might not exist

        if not results or not results.get("hits"):
            return None

        # Filter by pipeline_name and sort by created_at in Python
        matching_states = []
        for hit in results["hits"]:
            state = hit["document"]
            # Check if pipeline_name matches (handle both normalized and non-normalized field names)
            doc_pipeline_name = state.get(p_pipeline_name) or state.get("pipeline_name")
            if doc_pipeline_name == pipeline_name:
                matching_states.append(state)

        if not matching_states:
            return None

        # Sort by created_at descending (most recent first)
        def get_created_at(state_doc):
            created_at = state_doc.get(p_created_at) or state_doc.get("created_at")
            if isinstance(created_at, (int, float)):
                return created_at
            # Try to parse if it's a string
            try:
                return pendulum.parse(created_at).timestamp() if created_at else 0
            except Exception:
                return 0

        matching_states.sort(key=get_created_at, reverse=True)

        # Check if load was completed for each state (most recent first)
        for state in matching_states:
            load_id = state.get(p_dlt_load_id) or state.get("_dlt_load_id")
            if not load_id:
                continue

            # Check if load was completed
            try:
                # Fetch all loads and filter in Python
                load_search = {"q": "*", "per_page": 100}
                load_results = self._request(
                    "GET",
                    f"/collections/{loads_collection}/documents/search",
                    params=load_search,
                )
                if load_results and load_results.get("hits"):
                    for load_hit in load_results["hits"]:
                        load_doc = load_hit["document"]
                        doc_load_id = load_doc.get(p_load_id) or load_doc.get(
                            C_DLT_LOADS_TABLE_LOAD_ID
                        )
                        if doc_load_id == load_id:
                            return StateInfo.from_normalized_mapping(
                                state, self.schema.naming
                            )
            except TypesenseApiError:
                continue

        return None

    def get_stored_schema(
        self, schema_name: str = None
    ) -> t.Optional[StorageSchemaInfo]:
        version_collection = self.make_qualified_collection_name(
            self.schema.version_table_name
        )
        p_schema_name = self.schema.naming.normalize_identifier("schema_name")

        try:
            # Fetch all documents and filter in Python
            # This is more reliable than filter_by which requires fields to be facetable
            search_params = {
                "q": "*",
                "per_page": 100,
            }  # Get enough to find the right one

            results = self._request(
                "GET",
                f"/collections/{version_collection}/documents/search",
                params=search_params,
            )

            if not results or not results.get("hits"):
                return None

            # Filter by schema_name if provided
            if schema_name:
                for hit in results["hits"]:
                    doc = hit["document"]
                    # Check if schema_name matches (handle both normalized and non-normalized field names)
                    doc_schema_name = doc.get(p_schema_name) or doc.get("schema_name")
                    if doc_schema_name == schema_name:
                        return StorageSchemaInfo.from_normalized_mapping(
                            doc, self.schema.naming
                        )
                return None
            else:
                # Return the most recent schema
                return StorageSchemaInfo.from_normalized_mapping(
                    results["hits"][0]["document"], self.schema.naming
                )
        except TypesenseApiError:
            return None

    def get_stored_schema_by_hash(
        self, schema_hash: str
    ) -> t.Optional[StorageSchemaInfo]:
        version_collection = self.make_qualified_collection_name(
            self.schema.version_table_name
        )
        p_version_hash = self.schema.naming.normalize_identifier("version_hash")

        search_params = {
            "q": "*",
            "filter_by": f"{p_version_hash}:={schema_hash}",
            "per_page": 1,
        }

        try:
            results = self._request(
                "GET",
                f"/collections/{version_collection}/documents/search",
                params=search_params,
            )
        except TypesenseApiError:
            return None

        if results and results.get("hits"):
            return StorageSchemaInfo.from_normalized_mapping(
                results["hits"][0]["document"], self.schema.naming
            )
        return None

    def create_load_job(
        self,
        table: "PreparedTableSchema",
        file_path: str,
        load_id: str,
        restore: bool = False,
    ) -> "LoadJob":
        return LoadTypesenseJob(file_path, table, self)

    def complete_load(self, load_id: str) -> None:
        values = [
            load_id,
            self.schema.name,
            0,  # status
            pendulum.now().isoformat(),
            self.schema.version_hash,
        ]

        properties = {k: v for k, v in zip(self.loads_collection_properties, values)}

        for k, v in properties.items():
            if isinstance(v, str):
                try:
                    dt = pendulum.parse(v)
                    if isinstance(dt, pendulum.DateTime):
                        properties[k] = int(dt.timestamp())
                except Exception:
                    pass

        loads_collection = self.make_qualified_collection_name(
            self.schema.loads_table_name
        )
        self._request(
            "POST", f"/collections/{loads_collection}/documents", json_data=properties
        )

    def _create_sentinel_collection(self) -> None:
        schema = {
            "name": self.sentinel_collection,
            "fields": [{"name": "id", "type": "string"}],
        }
        self._request("POST", "/collections", json_data=schema)

    def _update_schema_in_storage(self, schema: Schema) -> None:
        schema_str = json.dumps(schema.to_dict())
        values = [
            schema.version,
            schema.ENGINE_VERSION,
            pendulum.now().isoformat(),
            schema.name,
            schema.stored_version_hash,
            schema_str,
        ]

        properties = {k: v for k, v in zip(self.version_collection_properties, values)}

        # Convert timestamp
        for k, v in properties.items():
            if isinstance(v, str):
                try:
                    dt = pendulum.parse(v)
                    if isinstance(dt, pendulum.DateTime):
                        properties[k] = int(dt.timestamp())
                except Exception:
                    pass

        version_collection = self.make_qualified_collection_name(
            self.schema.version_table_name
        )
        self._request(
            "POST", f"/collections/{version_collection}/documents", json_data=properties
        )

    def __enter__(self) -> "TypesenseClient":
        if self._session is None:
            self._session = requests.Session()
            self._session.headers.update(
                {
                    "X-TYPESENSE-API-KEY": self.api_key,
                    "Content-Type": "application/json",
                }
            )
        return self

    def __exit__(
        self,
        exc_type: t.Type[BaseException],
        exc_val: BaseException,
        exc_tb: t.Any,
    ) -> None:
        pass

    def import_documents(
        self, collection_name: str, jsonl_data: str, action: str
    ) -> str:
        """Imports documents using bulk import API"""
        params = {"action": action}
        # Send as UTF-8 encoded bytes (requests library requires this for non-ASCII characters)
        if isinstance(jsonl_data, str):
            jsonl_data = jsonl_data.encode("utf-8")
        return self._request(
            "POST",
            f"/collections/{collection_name}/documents/import",
            data=jsonl_data,
            params=params,
        )
