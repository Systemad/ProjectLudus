FROM python:3.12-slim-trixie

ENV PYTHONUNBUFFERED=1
#ENV PYTHONPATH=/opt/dagster/app

COPY --from=ghcr.io/astral-sh/uv:latest /uv /uvx /bin/

COPY pyproject.toml uv.lock ./
RUN uv pip install -r pyproject.toml --system

ENV DAGSTER_HOME=/opt/dagster/dagster_home
RUN mkdir -p $DAGSTER_HOME

COPY src/orchestration/dagster.yaml $DAGSTER_HOME/dagster.yaml
WORKDIR /opt/dagster/app
# copy full src tree so package layout is preserved
COPY src /opt/dagster/app/src
ENV PYTHONPATH="/opt/dagster/app/src:${PYTHONPATH}"

EXPOSE 4000

#HEALTHCHECK --timeout=1s --start-period=3s --interval=3s --retries=20 CMD ["dagster", "api", "grpc-health-check", "-p", "4000"]

# CMD allows this to be overridden from run launchers or executors to execute runs and steps
CMD ["dagster", "code-server", "start", "-h", "0.0.0.0", "-p", "4000", "-f", "src/orchestration/definitions.py"]