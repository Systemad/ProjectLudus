CREATE TABLE IF NOT EXISTS steam.tracked_games (
    game_id bigint NOT NULL,
    steam_app_id bigint NOT NULL,
    refreshed_at timestamptz NOT NULL DEFAULT now(),
    PRIMARY KEY (game_id, steam_app_id)
);
