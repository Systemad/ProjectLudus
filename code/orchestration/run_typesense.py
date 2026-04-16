import sys

sys.path.append("src")
from orchestration.defs.typesense_ingest.jobs import (
    company_typesense_source,
    company_typesense_source_pipeline,
    #    typesense_source,
    #    typesense_source_pipeline,
)

# print("RUNNING GAMES TYPESENSE PIPELINE")
# typesense_source_pipeline.run(typesense_source)
print("RUNNING COMPANY TYPESENSE PIPELINE")
company_typesense_source_pipeline.run(company_typesense_source)
print("ALL PIPELINES DONE")
