{{ codegen.create_base_models(
    source_name='igdb_raw',
    tables=[
        "games",
        "companies",
        "platforms",
        -- list all 60+ tables here
    ]
) }}
