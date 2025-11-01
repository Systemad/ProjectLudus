#!/bin/bash

set -e


DOCKER_USER="becknova"
PG_MAJOR_VERSION="17"
PG_SEARCH_TAG="v0.19.4-pg17"
DOCKERFILE_PATH="./docker/Dockerfile"
IMAGE_TAG="$DOCKER_USER/paradedb-ext:$PARADEDB_VERSION-$PG_VERSION"
PLATFORMS="linux/amd64,linux/arm64"
PGMQ_VERSION="1.7.0"

PUSH_FLAG=""
# --- Script Execution ---

# 1. Check for local changes before getting the SHA
#if [ -n "$(git status --porcelain)" ]; then
#  echo "🚨 WARNING: Uncommitted changes detected. Using the latest commit SHA."
#fi

# 2. Get the current Git commit SHA (to satisfy the ARG COMMIT_SHA)
#COMMIT_SHA=$(git rev-parse HEAD)
#echo "✅ Detected Commit SHA: $COMMIT_SHA"
  

echo "Logging in to Docker Hub..."
# You must have run 'docker login' manually first, or this will prompt for credentials.
if ! docker login; then
  echo "❌ Docker login failed. Please run 'docker login' and try again."
  exit 1
fi

if [ "$PUSH_FLAG" == "--push" ]; then
    echo "Push flag enabled."
fi


# Enable Docker BuildKit
export DOCKER_BUILDKIT=1

# Create and use a new builder instance
docker buildx create --name multiarch-builder --use || true

# 4. Build the Docker Image, passing the ARGs
echo "Building image $IMAGE_TAG..."
#docker build \


docker_command="docker buildx build \
  --build-arg PG_MAJOR_VERSION='${PG_MAJOR_VERSION}' \
  --build-arg PG_SEARCH_TAG='${PG_SEARCH_TAG}' \
  --build-arg PGMQ_VERSION='${PGMQ_VERSION}' \
  --file '${DOCKERFILE_PATH}' \
  --tag '${IMAGE_TAG}' \
  --platform='${PLATFORMS}' \
  ${PUSH_FLAG}
  ."
echo "Executing build command:"

echo "$docker_command"
eval "$docker_command"

echo "Build completed successfully!"
if [ "$PUSH_FLAG" == "--push" ]; then
    echo "Image tagged and pushed as: ${IMAGE_TAG}"
    #echo "Image also tagged and pushed as: ghcr.io/${REPO_NAME}:latest-pg${PG_MAJOR_VERSION}"
else
    echo "Image tagged locally as: ${IMAGE_TAG}"
    #echo "Image also tagged locally as: ghcr.io/${REPO_NAME}:latest-pg${PG_MAJOR_VERSION}"
fi

#if [ $? -ne 0 ]; then
#  echo "❌ Docker build failed."
#  exit 1
#fi




#if [ $? -eq 0 ]; then
#  echo "✨ SUCCESS! Image published as: $IMAGE_TAG"
#else
#  echo "❌ Docker push failed."
#  exit 1
#fi