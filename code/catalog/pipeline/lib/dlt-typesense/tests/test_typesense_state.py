import os
import pytest
import dlt
from dlt.common.utils import uniq_id
from dlt_typesense.typesense_client import TypesenseClient
from tests.pipeline.utils import assert_load_info


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_get_stored_state() -> None:
    """Test state retrieval with real Typesense."""
    pipeline_name = "test_state_pipeline_" + uniq_id()
    pipeline = dlt.pipeline(
        pipeline_name=pipeline_name,
        destination="dlt_typesense.factory.typesense",
        dataset_name="test_state_dataset_" + uniq_id(),
    )

    # Create multiple loads to test state storage
    # Use unique IDs for each load to avoid duplicate ID errors with append disposition
    @dlt.resource
    def users(load_num: int):
        yield [{"id": f"user_{load_num}", "name": f"Alice_{load_num}"}]

    # First load
    info1 = pipeline.run(users(1))
    assert_load_info(info1)

    # Second load
    info2 = pipeline.run(users(2))
    assert_load_info(info2)

    # Third load
    info3 = pipeline.run(users(3))
    assert_load_info(info3)

    # Retrieve state
    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        state = client.get_stored_state(pipeline_name)

    # Verify state exists and is correct
    assert state is not None
    assert state.pipeline_name == pipeline_name
    assert state._dlt_load_id is not None

    # Test edge case: non-existent pipeline
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        non_existent_state = client.get_stored_state(
            "non_existent_pipeline_" + uniq_id()
        )
        assert non_existent_state is None


@pytest.mark.skipif(
    not os.environ.get("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY"),
    reason="Typesense credentials not configured",
)
def test_uncommitted_state() -> None:
    """Test state restoration edge case where loads are incomplete (uncommitted state).

    Load uncommitted state into typesense, meaning that data is written to the state
    table but load is not completed (nothing is added to loads table).

    Ensure that state restoration does not include such state.
    """
    pipeline = dlt.pipeline(
        "uncommitted_state",
        destination="dlt_typesense.factory.typesense",
        dev_mode=True,
    )

    state_val = 0

    @dlt.resource
    def dummy_table():
        dlt.current.resource_state("dummy_table")["val"] = state_val
        yield [1, 2, 3]

    # Create > 10 load packages to be above pagination size when restoring state
    for _ in range(12):
        state_val += 1
        pipeline.extract(dummy_table)

    pipeline.normalize()
    info = pipeline.load()

    from dlt_typesense.typesense_client import TypesenseClient

    client: TypesenseClient
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        state = client.get_stored_state(pipeline.pipeline_name)

    assert state and state.version == state_val

    # Delete last 10 _dlt_loads entries so pagination is triggered when restoring state
    with pipeline.destination_client() as client:  # type: ignore[assignment]
        loads_collection = client.make_qualified_collection_name(
            pipeline.default_schema.loads_table_name
        )
        p_load_id = pipeline.default_schema.naming.normalize_identifier("load_id")

        # Fetch all loads to get document IDs
        load_search = {"q": "*", "per_page": 250}
        load_results = client._request(
            "GET",
            f"/collections/{loads_collection}/documents/search",
            params=load_search,
        )

        if load_results and load_results.get("hits"):
            # Get document IDs for loads we want to delete (last 10, excluding first 2)
            loads_to_delete = []
            for hit in load_results["hits"]:
                load_doc = hit.get("document", {})
                doc_load_id = load_doc.get(p_load_id) or load_doc.get("load_id")
                # Typesense returns document ID in the document itself (as "id" field)
                # or we can use the document's id field directly
                doc_id = load_doc.get("id")
                if doc_id and doc_load_id in info.loads_ids[2:]:
                    loads_to_delete.append(doc_id)

            # Delete documents by ID
            for doc_id in loads_to_delete:
                client._request(
                    "DELETE", f"/collections/{loads_collection}/documents/{doc_id}"
                )

    with pipeline.destination_client() as client:  # type: ignore[assignment]
        state = client.get_stored_state(pipeline.pipeline_name)

    # Latest committed state is restored (should be version 2, from the 2nd load)
    assert state and state.version == 2
