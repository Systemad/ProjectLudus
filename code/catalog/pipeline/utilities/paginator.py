from dlt.sources.helpers.rest_client.paginators import BasePaginator
from dlt.sources.rest_api.config_setup import register_paginator

class IGDBPaginator(BasePaginator):
    def __init__(self, limit=500):
        super().__init__()
        self.limit = limit
        self.offset = 0

    def update_state(self, response, data=None):
        count = len(data or [])

        if count < self.limit:
            self._has_next_page = False

        self.offset += count

    def update_request(self, request):
        if not isinstance(request.data, str):
            raise TypeError(
                "IGDBPaginator expects request.data to be a string"
            )

        parts = [
            p.strip()
            for p in request.data.split(";")
            if p.strip()
            and not p.strip().startswith("offset")
            and not p.strip().startswith("limit")
        ]

        parts.append(f"limit {self.limit}")
        parts.append(f"offset {self.offset}")

        request.data = "; ".join(parts) + ";"


register_paginator("igdb_offset", IGDBPaginator)
