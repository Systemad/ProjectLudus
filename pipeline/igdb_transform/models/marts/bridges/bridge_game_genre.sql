select game_id, genre_id from {{ ref("int_game_genre") }}
