{{ clean_lookup("stg_release_date_statuses",
   columns=["id","created_at","updated_at","name","description","checksum"],
   required_cols=["id","name"])
}}
