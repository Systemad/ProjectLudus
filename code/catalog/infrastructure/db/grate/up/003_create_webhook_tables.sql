create schema if not exists igdb_source;

create table webhook_events (
    id uuid primary key,
    entity_id bigint not null,
    received_at timestamptz not null default now(),
    endpoint text not null,
    event_type text not null,
    payload jsonb not null,
    processed boolean not null default false
);
