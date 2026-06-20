{{ clean_lookup("stg_language_support_types",
   columns=["id","created_at","updated_at","name","checksum"],
   required_cols=["id","name"])
}}
