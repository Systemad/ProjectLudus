from dlt.common.destination.exceptions import (
    DestinationTerminalException,
    DestinationTransientException,
)


class TypesenseException(DestinationTerminalException):
    """Base Typesense exception"""


class TypesenseConnectionError(DestinationTransientException):
    """Connection failed - will retry"""


class TypesenseApiError(TypesenseException):
    """API returned error - check status code for retry decision"""


class TypesenseSchemaError(TypesenseException):
    """Schema validation failed - terminal"""


class TypesenseBulkImportError(TypesenseException):
    """Bulk import had failures - includes failed documents"""


class TypesenseMissingCredentialsError(TypesenseException):
    """Required credentials are missing"""
