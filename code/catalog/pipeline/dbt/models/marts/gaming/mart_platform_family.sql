{{ clean_lookup("stg_platform_family",
   columns=["id","name","slug","checksum"],
   required_cols=["id","name","slug"])
}}
