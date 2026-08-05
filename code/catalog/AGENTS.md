# Catalog Pipeline and Read Model

This directory acquires and transforms catalog data consumed by `code/app/backend/Catalog`.

```text
IGDB/Steam → dlt → dbt staging → intermediate → marts/bridges → grate → PostgreSQL → scaffold
```

## Rules

- Complete dbt, grate, and scaffolding before exposing new Catalog fields/API endpoints.
- `Data/` is database-first scaffolded output; never edit it. Run `Invoke-Scaffold` after schema changes.
- Catalog is an in-process Backend.API module, not a deployed service.
- Play and Notifications own their separate code-first EF schemas; do not place them here.
- Keep dbt layers ordered staging → intermediate → marts/bridges.
- Validate referenced game/company IDs with inner joins.
- Bridges use one target-side FK, deterministic `DISTINCT ON`, and YAML `alias:` to remove `bridge_`.

## New IGDB data

1. Add the endpoint/resource to `pipeline/endpoints.json` and dlt.
2. Add staging metadata, marts, bridge SQL, and YAML contracts.
3. Run dbt, apply grate, then scaffold.
4. Add the Catalog projection/endpoint and regenerate clients.

## Commands

```powershell
# from code/catalog/
Invoke-DbtBuild
Invoke-Scaffold
```

AppHost provides `catalogdb` to Catalog/Notifications but does not migrate catalog schema. Common fixes: split IGDB requests causing 413s, use deterministic bridge deduplication, recreate stale Prefect deployments, and apply explicit schema changes before incremental FK updates.
