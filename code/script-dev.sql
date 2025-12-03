START TRANSACTION;
CREATE EXTENSION IF NOT EXISTS pg_search;

CREATE TABLE companies (
    id bigint NOT NULL,
    name text NOT NULL,
    slug text NOT NULL,
    url text NOT NULL,
    CONSTRAINT pk_companies PRIMARY KEY (id)
);

CREATE TABLE games (
    id bigint NOT NULL,
    name text NOT NULL,
    slug text NOT NULL,
    CONSTRAINT pk_games PRIMARY KEY (id)
);

CREATE TABLE developer_game (
    company_id bigint NOT NULL,
    game_id bigint NOT NULL,
    CONSTRAINT pk_developer_game PRIMARY KEY (company_id, game_id),
    CONSTRAINT fk_developer_game_companies_company_id FOREIGN KEY (company_id) REFERENCES companies (id) ON DELETE CASCADE,
    CONSTRAINT fk_developer_game_games_game_id FOREIGN KEY (game_id) REFERENCES games (id) ON DELETE CASCADE
);

COMMIT;

