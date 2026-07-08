ALTER TABLE steam_raw.concurrent_users
SET (timescaledb.compress, timescaledb.compress_segmentby = 'game_id');
ALTER TABLE steam_raw.store_pricing
SET (timescaledb.compress, timescaledb.compress_segmentby = 'steam_app_id');

SELECT add_compression_policy('steam_raw.concurrent_users', INTERVAL '7 days');
SELECT add_compression_policy('steam_raw.store_pricing', INTERVAL '7 days');

SELECT add_retention_policy('steam_raw.concurrent_users', INTERVAL '180 days');
SELECT add_retention_policy('steam_raw.store_pricing', INTERVAL '365 days');
