{{ clean_lookup("stg_player_perspectives",
   columns=["id","created_at","updated_at","name","slug","url","checksum"],
   required_cols=["id","name","slug"])
}}
