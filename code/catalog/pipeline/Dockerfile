FROM prefecthq/prefect:3-python3.13 AS runtime
WORKDIR /app

ENV UV_LINK_MODE=copy

COPY pyproject.toml uv.lock ./
COPY lib/dlt-typesense lib/dlt-typesense
RUN --mount=type=cache,target=/root/.cache/uv \
    uv sync --no-dev --frozen

COPY . .

ENV VIRTUAL_ENV=/app/.venv
ENV PATH="/app/.venv/bin:$PATH"
RUN cd dbt && uv run dbt deps
