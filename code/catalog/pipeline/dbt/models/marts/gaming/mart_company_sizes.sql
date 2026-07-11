{{ clean_lookup("stg_company_sizes",
   columns=["id","created_at","updated_at","name","checksum"],
   required_cols=["id","name"])
}}
