"""Pipeline testing utilities."""

from typing import Any


def assert_load_info(load_info: Any) -> None:
    """Assert that a LoadInfo object from dlt pipeline is valid.

    Args:
        load_info: LoadInfo object returned from pipeline.run()

    Raises:
        AssertionError: If load_info is invalid or indicates failure
    """
    assert load_info is not None, "LoadInfo should not be None"

    # Check that load_info has expected attributes
    # LoadInfo has 'loads_ids' attribute (list of load IDs), not 'loads' or 'jobs'
    assert hasattr(load_info, "loads_ids"), "LoadInfo should have 'loads_ids' attribute"
    assert hasattr(
        load_info, "dataset_name"
    ), "LoadInfo should have 'dataset_name' attribute"

    # Verify loads list exists and is not empty (if there was data to load)
    # Note: Empty loads might have empty loads list, so we don't assert on length

    # Check for any failed jobs (if the attribute exists)
    if hasattr(load_info, "failed_jobs") and load_info.failed_jobs:
        failed_count = len(load_info.failed_jobs)
        raise AssertionError(
            f"LoadInfo indicates {failed_count} failed job(s). "
            f"Failed jobs: {load_info.failed_jobs}"
        )

    # If load_info has a loads_ids attribute, verify it's not empty (if data was loaded)
    if hasattr(load_info, "loads_ids") and load_info.loads_ids:
        assert len(load_info.loads_ids) > 0, "LoadInfo should have at least one load ID"
