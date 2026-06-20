import json
from pathlib import Path

_PATH = Path(__file__).resolve().parent.parent.parent.parent / "endpoints.json"

with open(_PATH) as f:
    _DATA = json.load(f)


def get_ref() -> list[str]:
    return _DATA["ref"]


def get_default() -> list[str]:
    return _DATA["default"]
