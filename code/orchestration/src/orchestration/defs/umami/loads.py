import os

import dlt
from dlt.sources.rest_api import rest_api_source
from dlt.sources.helpers.rest_client.auth import BearerTokenAuth
from dotenv import load_dotenv

from .sources.umami_source import umami_source

load_dotenv(override=True)

base_url = os.getenv("UMAMI_BASE_URL")
website_id = os.getenv("UMAMI_WEBSITE_ID")
api_key = os.getenv("UMAMI_API_KEY")

if not base_url or not website_id or not api_key:
    raise ValueError(
        "Missing required environment variables: UMAMI_BASE_URL, UMAMI_WEBSITE_ID, and/or UMAMI_API_KEY"
    )

config = umami_source(
    base_url=base_url,
    website_id=website_id,
)

# Inject auth into client config
config["client"]["auth"] = BearerTokenAuth(token=api_key)

umami_source_instance = rest_api_source(
    name="umami",
    config=config,
)

pipeline = dlt.pipeline(
    pipeline_name="umami_analytics",
    destination="postgres",
    dataset_name="analytics",
)
