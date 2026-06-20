{{ clean_lookup("stg_platform_types",
   columns=["id","name","created_at","updated_at","checksum"],
   required_cols=["id","name"])
}}
