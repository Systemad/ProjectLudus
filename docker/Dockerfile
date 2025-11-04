ARG PG_SEARCH_TAG=v0.19.4-pg17
ARG PGMQ_VERSION=1.7.0

FROM paradedb/paradedb:${PG_SEARCH_TAG} AS builder-pg_search

ARG PGMQ_VERSION

WORKDIR /tmp
RUN apt-get update && apt-get install -y --no-install-recommends \
    wget \
    make \
    unzip && \
    rm -rf /var/lib/apt/lists/* 

RUN wget -O pgmq-${PGMQ_VERSION}.zip https://api.pgxn.org/dist/pgmq/${PGMQ_VERSION}/pgmq-${PGMQ_VERSION}.zip && \
    unzip pgmq-${PGMQ_VERSION}.zip
    
WORKDIR /tmp/pgmq-${PGMQ_VERSION}
RUN make && make install

WORKDIR /
RUN apt-get purge -y wget make unzip && \
    apt-get autoremove -y && \
    rm -rf /tmp/pgmq-${PGMQ_VERSION} && \
    rm -rf /var/lib/apt/lists/*
