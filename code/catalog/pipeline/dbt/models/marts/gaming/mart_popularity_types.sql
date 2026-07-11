{{ clean_lookup("stg_popularity_types",
   columns=["id","name","created_at","updated_at"],
   required_cols=["id","name"])
}}
