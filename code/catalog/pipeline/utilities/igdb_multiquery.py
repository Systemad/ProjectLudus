MULTIQUERY_URL = "https://api.igdb.com/v4/multiquery"
# The five popularity lists can produce up to 1,500 candidate games. IGDB
# multiquery accepts at most 10 queries, so keep external-game batches within
# that ceiling while leaving room for multiple Steam records per game.
BATCH_SIZE = 150
EXTERNAL_GAME_QUERY_LIMIT = 500

POP_TYPES = {
    "p5": {"popularity_type": 5, "limit": 500},
    "p6": {"popularity_type": 6, "limit": 250},
    "p8": {"popularity_type": 8, "limit": 250},
    "p9": {"popularity_type": 9, "limit": 250},
    "p10": {"popularity_type": 10, "limit": 250},
}


def build_pop_multiquery() -> str:
    queries = (
        f'query popularity_primitives "{name}" {{ fields *; where popularity_type = {cfg["popularity_type"]}; sort value desc; limit {cfg["limit"]}; }}'
        for name, cfg in POP_TYPES.items()
    )
    return ";".join(queries) + ";"


def build_external_games_multiquery(batches: list[list[int]]) -> str:
    queries = (
        f'query external_games "batch{i}" {{ fields game, uid; where game = ({",".join(str(gid) for gid in batch)}) & external_game_source = 1; limit {EXTERNAL_GAME_QUERY_LIMIT}; }}'
        for i, batch in enumerate(batches)
    )
    return ";".join(queries) + ";"


def flatten_multiquery(row: dict) -> list[dict]:
    return row.get("result", [])
