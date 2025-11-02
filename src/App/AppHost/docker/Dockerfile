ARG PG_MAJOR_VERSION=17
ARG PG_SEARCH_TAG=v0.19.4-pg17
ARG PGMQ_VERSION=1.7.0
FROM paradedb/paradedb:${PG_SEARCH_TAG} AS builder-pg_search

WORKDIR /tmp
RUN apt-get update && apt-get install -y --no-install-recommends \
    build-essential \
    wget \
    make \
    unzip \
    gcc && \
    wget -q https://github.com/unicode-org/icu/releases/download/release-76-1/icu4c-76_1-src.tgz && \
    tar xzvf icu4c-76_1-src.tgz && \
    rm -rf icu4c-76_1-src.tgz && \
    rm -rf /var/lib/apt/lists/* 

WORKDIR /tmp
RUN wget -O pgmq-PGMQ_VERSION.zip https://api.pgxn.org/dist/pgmq/PGMQ_VERSION/pgmq-PGMQ_VERSION.zip && \
    unzip pgmq-PGMQ_VERSION.zip
WORKDIR /tmp/pgmq-PGMQ_VERSION
RUN make && make install

WORKDIR /
RUN echo "deb http://apt.postgresql.org/pub/repos/apt/ bookworm-pgdg main" | tee /etc/apt/sources.list.d/pgdg.list && \
    wget -qO - https://www.postgresql.org/media/keys/ACCC4CF8.asc | apt-key add - && \
    apt-get purge -y build-essential wget make gcc unzip && \
    apt-get autoremove -y && \
    rm -rf /tmp/pgmq-PGMQ_VERSION && \
    rm -rf /var/lib/apt/lists/*
