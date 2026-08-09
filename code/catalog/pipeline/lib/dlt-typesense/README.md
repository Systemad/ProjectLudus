# dlt-typesense

A [dlt](https://dlthub.com/) destination for loading data into [Typesense](https://typesense.org/), an open-source, typo-tolerant search engine.

This library is used by the Project Ludus catalog pipeline to publish search documents. Typesense write credentials belong in the deployment environment (for example Dokploy/Prefect), not in source control. The mobile app receives only its read-only search configuration through Expo public environment variables.

## Features

- **Write Dispositions**: `append`, `replace`, and `merge` (upsert) operations
- **Schema Management**: Automatic collection creation and updates
- **State Sync**: Pipeline state stored in special collections (`_dlt_*`)
- **Type Mapping**: Automatic mapping from dlt types to Typesense types (text→string, timestamp→int64)
- **Field Configuration**: Configure faceting, sorting, and indexing via adapter

## Installation

```bash
pip install dlt-typesense
```

## Quick Start

```python
import dlt
from dlt_typesense.typesense_adapter import typesense_adapter

@dlt.resource
def users():
    yield [
        {"id": "1", "name": "Alice", "age": 30},
        {"id": "2", "name": "Bob", "age": 25},
    ]

# Configure field options
users_r = typesense_adapter(users, facet=["age"], sort=["name", "age"])

# Run pipeline
pipeline = dlt.pipeline(
    pipeline_name="my_pipeline",
    destination="typesense",
    dataset_name="my_dataset",
)

info = pipeline.run(users_r)
```

## Configuration

### Prerequisites

- A Typesense instance (self-hosted or cloud)
- API key with write access

### Setup

Configure the destination in `secrets.toml` or via environment variables.

**Using `secrets.toml`:**

```toml
[destination.typesense.credentials]
url = "http://localhost:8108"
api_key = "your-api-key"
```

**Using environment variables:**

```bash
export DESTINATION__TYPESENSE__CREDENTIALS__URL=http://localhost:8108
export DESTINATION__TYPESENSE__CREDENTIALS__API_KEY=your-api-key
```

### Batch Size

Configure the batch size for bulk import operations:

```toml
[destination.typesense]
import_batch_size = 1000
```

## Usage

### Basic Pipeline

```python
import dlt

@dlt.resource
def users():
    yield [
        {"id": "1", "name": "Alice", "age": 30},
        {"id": "2", "name": "Bob", "age": 25},
    ]

pipeline = dlt.pipeline(
    pipeline_name="users_pipeline",
    destination="typesense",
    dataset_name="users_dataset",
)

pipeline.run(users())
```

### Using the Adapter

The `typesense_adapter` allows you to configure collection field properties:

```python
from dlt_typesense.typesense_adapter import typesense_adapter

@dlt.resource
def products():
    yield [
        {"id": "1", "name": "Laptop", "price": 999, "category": "Electronics"},
        {"id": "2", "name": "Phone", "price": 699, "category": "Electronics"},
    ]

# Enable faceting on category and price, sorting on name and price
products_r = typesense_adapter(
    products,
    facet=["category", "price"],
    sort=["name", "price"]
)

pipeline.run(products_r)
```

**Adapter Parameters:**
- `facet`: Columns to enable faceting for (useful for filtering)
- `sort`: Columns to enable sorting for
- `index`: Columns to explicitly enable indexing for

### Write Dispositions

```python
# Append (default)
pipeline.run(users(), write_disposition="append")

# Replace (truncate and insert)
pipeline.run(users(), write_disposition="replace")

# Merge (upsert by id field)
pipeline.run(users(), write_disposition="merge")
```

## Limitations

- **Nested Objects**: Typesense flattens nested objects by default. Complex types are mapped to `object` type. For nested field support, enable `enable_nested_fields` in your Typesense configuration.
- **Arrays**: Typesense supports arrays (e.g., `string[]`, `int64[]`).
- **IDs**: Typesense uses the `id` field as the document identifier. dlt automatically handles this mapping for `merge` disposition.

## Development

### Running Tests

#### Using the Helper Script

A helper script is provided to automatically start a Typesense Docker instance and run tests:

```bash
# Run all tests
./run_integration_test.sh

# Run a specific test file
./run_integration_test.sh tests/test_typesense_configuration.py
```

The script will:
- Start a Typesense Docker container on port 8108
- Wait for Typesense to be ready
- Run the specified tests (or all tests if no file is provided)
- Clean up the container after tests complete (optional, currently commented out)

#### Manual Setup

Alternatively, you can manually start a local Typesense instance:

```bash
# Set API key
export TYPESENSE_API_KEY=xyz

# Create data directory
mkdir -p "$(pwd)"/typesense-data

# Start Typesense container
docker rm -f typesense-test 2>/dev/null || true
docker run -d --name typesense-test -p 8108:8108 \
  -v"$(pwd)"/typesense-data:/data \
  typesense/typesense:29.0 \
  --data-dir /data --api-key=$TYPESENSE_API_KEY --enable-cors

# Wait for Typesense to be ready
sleep 3
curl http://localhost:8108/health

# Set credentials for tests
export DESTINATION__TYPESENSE__CREDENTIALS__URL=http://localhost:8108
export DESTINATION__TYPESENSE__CREDENTIALS__API_KEY=xyz

# Run tests
uv run pytest tests/ -v
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.


