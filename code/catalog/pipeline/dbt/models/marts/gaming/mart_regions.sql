{{ clean_lookup("stg_regions",
   columns=["id","created_at","updated_at","name","category","identifier","checksum"],
   required_cols=["id","name"])
}}
