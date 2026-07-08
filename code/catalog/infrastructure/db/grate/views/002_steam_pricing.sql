DROP MATERIALIZED VIEW IF EXISTS steam.steam_pricing_hourly CASCADE;
CREATE MATERIALIZED VIEW steam.steam_pricing_hourly
WITH (timescaledb.continuous = true) AS
SELECT
    game_id,
    time_bucket('1 hour', captured_at) AS bucket,
    MIN(final_cents) AS min_price,
    MAX(final_cents) AS max_price,
    AVG(final_cents)::int AS avg_price
FROM steam_raw.store_pricing
GROUP BY 1, 2;

SELECT add_continuous_aggregate_policy('steam.steam_pricing_hourly',
    start_offset => INTERVAL '3 days',
    end_offset => INTERVAL '1 hour',
    schedule_interval => INTERVAL '1 hour');

DROP MATERIALIZED VIEW IF EXISTS steam.steam_pricing_daily CASCADE;
CREATE MATERIALIZED VIEW steam.steam_pricing_daily
WITH (timescaledb.continuous = true) AS
SELECT
    game_id,
    time_bucket('1 day', captured_at) AS bucket,
    MIN(final_cents) AS min_price,
    MAX(final_cents) AS max_price,
    AVG(final_cents)::int AS avg_price
FROM steam_raw.store_pricing
GROUP BY 1, 2;

SELECT add_continuous_aggregate_policy('steam.steam_pricing_daily',
    start_offset => INTERVAL '1 month',
    end_offset => INTERVAL '1 hour',
    schedule_interval => INTERVAL '1 day');
