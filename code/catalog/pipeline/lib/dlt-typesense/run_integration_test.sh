#!/bin/bash
# Script to start Typesense Docker instance and run tests
# Usage: ./run_integration_test.sh [test_file]
#   - If test_file is provided, runs only that test file
#   - If no argument is provided, runs all tests in tests/

set -e

# Get optional test file parameter
TEST_FILE="${1:-}"

# Set API key
export TYPESENSE_API_KEY=xyz

# Create data directory
mkdir -p "$(pwd)"/typesense-data

# Remove existing container if it exists
docker rm -f typesense-test 2>/dev/null || true

# Start Typesense container
echo "Starting Typesense container..."
docker run -d --name typesense-test -p 8108:8108 \
  -v"$(pwd)"/typesense-data:/data \
  typesense/typesense:29.0 \
  --data-dir /data --api-key=$TYPESENSE_API_KEY --enable-cors

# Wait for Typesense to be ready
echo "Waiting for Typesense to be ready..."
sleep 3

# Check health
echo "Checking Typesense health..."
curl http://localhost:8108/health || {
    echo "Typesense health check failed!"
    exit 1
}

# Set credentials for tests
export DESTINATION__TYPESENSE__CREDENTIALS__URL=http://localhost:8108
export DESTINATION__TYPESENSE__CREDENTIALS__API_KEY=xyz

# Run tests
if [ -n "$TEST_FILE" ]; then
    echo "Running test file: $TEST_FILE"
    uv run pytest "$TEST_FILE" -v
else
    echo "Running all tests..."
    uv run pytest tests/ -v
fi

# Cleanup (optional - comment out if you want to keep the container running)
# docker rm -f typesense-test
