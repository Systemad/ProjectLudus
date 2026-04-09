#!/usr/bin/env python3
"""Run IGDB debug incremental source with a dedicated debug pipeline."""

import json
from pathlib import Path

import dlt

from src.orchestration.defs.igdb_rest_ingest.loads_incremental_debug import (
    DEBUG_SOURCE_NAME,
    debug_incremental_source,
)

DEBUG_PIPELINE_NAME = "igdb_default_pipeline_debug_incremental"
DEBUG_DATASET_NAME = "igdb_source_debug_incremental"


def build_debug_pipeline():
    return dlt.pipeline(
        pipeline_name=DEBUG_PIPELINE_NAME,
        destination="postgres",
        dataset_name=DEBUG_DATASET_NAME,
        progress="log",
        dev_mode=False,
    )


def read_cursors():
    state_file = Path.home() / ".dlt" / "pipelines" / DEBUG_PIPELINE_NAME / "state.json"
    if not state_file.exists():
        return {}

    with state_file.open("r", encoding="utf-8") as f:
        state = json.load(f)

    resources = state.get("sources", {}).get(DEBUG_SOURCE_NAME, {}).get("resources", {})
    out = {}
    for name, resource_state in resources.items():
        updated = resource_state.get("incremental", {}).get("updated_at", {})
        if updated:
            out[name] = updated.get("last_value")
    return out


def run_once(label):
    print(f"\n=== {label} ===")
    pipeline = build_debug_pipeline()
    load_info = pipeline.run(debug_incremental_source)
    print(load_info)


print("=" * 80)
print("IGDB Debug Pipeline - Incremental Load Test")
print("=" * 80)

try:
    print("\n[1] Debug source and pipeline")
    print(f"  - Pipeline: {DEBUG_PIPELINE_NAME}")
    print(f"  - Dataset: {DEBUG_DATASET_NAME}")
    print(f"  - Source: {DEBUG_SOURCE_NAME}")

    print("\n[2] Cursors before run 1")
    print(read_cursors())

    run_once("RUN 1")

    print("\n[3] Cursors after run 1")
    print(read_cursors())

    run_once("RUN 2")

    print("\n[4] Cursors after run 2")
    print(read_cursors())

    print("\nDone")
except Exception as e:
    import sys

    print("\nError during pipeline run:")
    print(f"  {type(e).__name__}: {e}")
    import traceback

    traceback.print_exc()
    sys.exit(1)
