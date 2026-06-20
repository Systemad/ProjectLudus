import dataclasses
from typing import Final, Optional

from dlt.common.configuration import configspec
from dlt.common.configuration.specs.base_configuration import (
    CredentialsConfiguration,
)
from dlt.common.destination.client import DestinationClientDwhConfiguration
from dlt.common.utils import digest128

# Default configuration constants
DEFAULT_CONNECTION_TIMEOUT_SECONDS: float = 10.0
DEFAULT_BATCH_SIZE: int = 1000
DEFAULT_IMPORT_BATCH_SIZE: int = 40
DEFAULT_DATASET_SEPARATOR: str = "___"


@configspec
class TypesenseCredentials(CredentialsConfiguration):
    url: Optional[str] = None
    api_key: Optional[str] = None
    connection_timeout_seconds: float = DEFAULT_CONNECTION_TIMEOUT_SECONDS

    def on_resolved(self) -> None:
        from dlt_typesense.exceptions import TypesenseMissingCredentialsError

        if not self.url:
            raise TypesenseMissingCredentialsError("Typesense URL is required. Please provide 'url' in credentials configuration.")
        if not self.api_key:
            raise TypesenseMissingCredentialsError("Typesense API key is required. Please provide 'api_key' in credentials configuration.")

        if self.url.endswith("/"):
            self.url = self.url[:-1]

    def __str__(self) -> str:
        return self.url


@configspec
class TypesenseClientConfiguration(DestinationClientDwhConfiguration):
    destination_type: Final[str] = dataclasses.field(default="typesense", init=False, repr=False, compare=False)  # type: ignore
    credentials: TypesenseCredentials = None

    def on_resolved(self) -> None:
        from dlt_typesense.exceptions import TypesenseMissingCredentialsError

        if not self.credentials:
            raise TypesenseMissingCredentialsError("Typesense credentials are required. Please provide credentials configuration.")

    # Batch size for bulk import (number of documents per request)
    batch_size: int = DEFAULT_BATCH_SIZE
    # Batch size for import loop inside the destination job
    import_batch_size: int = DEFAULT_IMPORT_BATCH_SIZE
    dataset_separator: str = DEFAULT_DATASET_SEPARATOR

    def fingerprint(self) -> str:
        """Returns a fingerprint of the destination location"""
        if self.credentials and self.credentials.url:
            return digest128(self.credentials.url)
        return ""
