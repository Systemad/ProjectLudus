DROP MATERIALIZED VIEW IF EXISTS steam.steam_player_stats_hourly CASCADE;
CREATE MATERIALIZED VIEW steam.steam_player_stats_hourly
WITH (timescaledb.continuous = true) AS
SELECT
    game_id,
    time_bucket('1 hour', captured_at) AS bucket,
    MAX(current_players)::int AS peak_players,
    AVG(current_players)::int AS avg_players
FROM steam_raw.concurrent_users
GROUP BY 1, 2;

SELECT add_continuous_aggregate_policy('steam.steam_player_stats_hourly',
    start_offset => INTERVAL '3 days',
    end_offset => INTERVAL '1 hour',
    schedule_interval => INTERVAL '1 hour');

DROP MATERIALIZED VIEW IF EXISTS steam.steam_player_stats_daily CASCADE;
CREATE MATERIALIZED VIEW steam.steam_player_stats_daily
WITH (timescaledb.continuous = true) AS
SELECT
    game_id,
    time_bucket('1 day', captured_at) AS bucket,
    MAX(current_players)::int AS peak_players,
    AVG(current_players)::int AS avg_players
FROM steam_raw.concurrent_users
GROUP BY 1, 2;

SELECT add_continuous_aggregate_policy('steam.steam_player_stats_daily',
    start_offset => INTERVAL '1 month',
    end_offset => INTERVAL '1 hour',
    schedule_interval => INTERVAL '1 day');
