TRACKED_GAMES_SQL = "SELECT game_id, steam_app_id FROM steam.tracked_games"


def get_tracked_games(conn) -> list[tuple[int, int]]:
    rows = conn.execute(TRACKED_GAMES_SQL).fetchall()
    return [(r[0], r[1]) for r in rows]
