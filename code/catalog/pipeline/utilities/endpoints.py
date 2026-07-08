import json
from pathlib import Path

_PATH = Path(__file__).resolve().parent.parent / "endpoints.json"


def get_ref() -> list[str]:
    with open(_PATH) as f:
        return json.load(f)["ref"]


def get_default() -> list[str]:
    with open(_PATH) as f:
        return json.load(f)["default"]
