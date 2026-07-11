{{ clean_lookup("stg_company_types",
   columns=["id","created_at","updated_at","name","checksum"],
   required_cols=["id","name"])
}}
