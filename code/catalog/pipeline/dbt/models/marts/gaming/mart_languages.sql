{{ clean_lookup("stg_languages",
   columns=["id","created_at","updated_at","name","native_name","locale","checksum"],
   required_cols=["id","name"])
}}
