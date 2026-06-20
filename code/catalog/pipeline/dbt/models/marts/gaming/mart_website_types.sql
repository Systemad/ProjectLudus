{{ clean_lookup("stg_website_types",
   columns=["id","created_at","updated_at","type","checksum"],
   required_cols=["id","type"])
}}
