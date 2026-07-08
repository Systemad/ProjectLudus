CREATE SCHEMA IF NOT EXISTS steam_raw;
CREATE SCHEMA IF NOT EXISTS steam;
CREATE SCHEMA IF NOT EXISTS umami_raw;

-- steam_raw.concurrent_users (hypertable)
CREATE TABLE IF NOT EXISTS steam_raw.concurrent_users (
    game_id bigint NOT NULL,
    steam_app_id bigint NOT NULL,
    current_players bigint NOT NULL,
    captured_at timestamptz NOT NULL,
    PRIMARY KEY (game_id, captured_at)
);
SELECT create_hypertable('steam_raw.concurrent_users', 'captured_at',
    chunk_time_interval => interval '24 hours', if_not_exists => true);

-- steam_raw.store_pricing (hypertable)
CREATE TABLE IF NOT EXISTS steam_raw.store_pricing (
    game_id bigint NOT NULL,
    steam_app_id bigint NOT NULL,
    initial_cents int NOT NULL,
    final_cents int NOT NULL,
    discount_percent int NOT NULL DEFAULT 0,
    currency text NOT NULL,
    initial_formatted text NOT NULL DEFAULT '',
    final_formatted text NOT NULL DEFAULT '',
    captured_at timestamptz NOT NULL,
    PRIMARY KEY (steam_app_id, captured_at)
);
SELECT create_hypertable('steam_raw.store_pricing', 'captured_at',
    chunk_time_interval => interval '7 days', if_not_exists => true);

-- steam_raw.store_details (regular table, append-only)
CREATE TABLE IF NOT EXISTS steam_raw.store_details (
    game_id bigint NOT NULL,
    steam_app_id bigint NOT NULL,
    header_url text NOT NULL DEFAULT '',
    capsule_url text NOT NULL DEFAULT '',
    captured_at timestamptz NOT NULL DEFAULT now(),
    PRIMARY KEY (steam_app_id, captured_at)
);

-- steam_raw.reviews (regular table, append-only)
CREATE TABLE IF NOT EXISTS steam_raw.reviews (
    game_id bigint NOT NULL,
    steam_app_id bigint NOT NULL,
    num_reviews int NOT NULL DEFAULT 0,
    review_score int NOT NULL DEFAULT 0,
    review_score_desc text NOT NULL DEFAULT '',
    total_positive int NOT NULL DEFAULT 0,
    total_negative int NOT NULL DEFAULT 0,
    total_reviews int NOT NULL DEFAULT 0,
    captured_at timestamptz NOT NULL DEFAULT now(),
    PRIMARY KEY (steam_app_id, captured_at)
);

-- umami_raw.pageviews (hypertable)
CREATE TABLE IF NOT EXISTS umami_raw.pageviews (
    game_id bigint NOT NULL,
    pageviews bigint NOT NULL,
    date date NOT NULL,
    PRIMARY KEY (game_id, date)
);
SELECT create_hypertable('umami_raw.pageviews', 'date',
    chunk_time_interval => interval '7 days', if_not_exists => true);
