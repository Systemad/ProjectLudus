import dlt
import niquests
from utilities.endpoints import get_ref
from utilities.igdb_client import get_igdb_headers
from utilities.igdb_multiquery import MULTIQUERY_URL

BATCH_SIZE = 10


def build_multiquery(tables: list[str]) -> str:
    return ";".join(f'query {t} "{t}" {{ fields *; limit 500; }}' for t in tables) + ";"


@dlt.resource(write_disposition="merge", primary_key="id")
def igdb_reference_tables():
    ref = get_ref()
    for i in range(0, len(ref), BATCH_SIZE):
        batch = ref[i : i + BATCH_SIZE]
        resp = niquests.post(
            MULTIQUERY_URL,
            headers=get_igdb_headers(),
            data=build_multiquery(batch),
            timeout=30,
        )
        resp.raise_for_status()
        for entry in resp.json():
            for r in entry.get("result", []):
                yield dlt.mark.with_table_name(r, entry["name"])


def run():
    dlt.pipeline(
        pipeline_name="igdb_ref_data",
        destination="postgres",
        dataset_name="igdb_ref",
    ).run(igdb_reference_tables())
