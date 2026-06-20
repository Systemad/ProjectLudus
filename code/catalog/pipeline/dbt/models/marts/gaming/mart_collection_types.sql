{{ clean_lookup("stg_collection_types",
   columns=["id","created_at","updated_at","name","description","checksum"],
   required_cols=["id","name"])
}}
