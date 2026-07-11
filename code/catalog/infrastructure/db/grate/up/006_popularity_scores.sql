CREATE TABLE IF NOT EXISTS steam_raw.popularity_scores (
    game_id bigint NOT NULL,
    popularity_type bigint NOT NULL,
    value double precision NOT NULL,
    calculated_at bigint NOT NULL,
    fetched_at timestamptz NOT NULL DEFAULT now()
);
