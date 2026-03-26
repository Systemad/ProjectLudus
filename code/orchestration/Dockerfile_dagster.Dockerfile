FROM python:3.12-slim-trixie
COPY --from=ghcr.io/astral-sh/uv:latest /uv /uvx /bin/
#RUN pip install uv
RUN uv pip install --system \
    dagster \
    dagster-graphql \
    dagster-webserver \
    dagster-postgres \
    dagster-docker
    
ENV DAGSTER_HOME=/opt/dagster/dagster_home/
RUN mkdir -p $DAGSTER_HOME

COPY src/orchestration/dagster.yaml $DAGSTER_HOME/
COPY workspace.yaml $DAGSTER_HOME/

WORKDIR $DAGSTER_HOME

#ENV DAGSTER_HOME=/opt/dagster/dagster_home/
#RUN mkdir -p $DAGSTER_HOME
#COPY ./src/orchestration/dagster.yaml $DAGSTER_HOME
#COPY dg.toml $DAGSTER_HOME
#WORKDIR $DAGSTER_HOME