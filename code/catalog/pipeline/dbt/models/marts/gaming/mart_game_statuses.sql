{{ clean_lookup("stg_game_statuses",
   columns=["id","created_at","updated_at","status","checksum"],
   required_cols=["id","status"])
}}
