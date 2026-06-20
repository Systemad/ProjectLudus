{{ clean_lookup("stg_date_formats",
   columns=["id","created_at","updated_at","format","checksum"],
   required_cols=["id","format"])
}}
