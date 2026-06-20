{{ clean_lookup("stg_genres",
   columns=["id","created_at","updated_at","name","slug","url","checksum"],
   required_cols=["id","name","slug"])
}}
