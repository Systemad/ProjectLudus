select game_id, player_perspective_id from {{ ref("int_game_player_perspective") }}
