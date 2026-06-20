{{ clean_lookup("stg_release_date_regions",
   columns=["id","created_at","updated_at","region","checksum"],
   required_cols=["id","region"])
}}
