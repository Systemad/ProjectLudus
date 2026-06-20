{{ clean_lookup("stg_game_release_formats",
   columns=["id","created_at","updated_at","format","checksum"],
   required_cols=["id","format"])
}}
