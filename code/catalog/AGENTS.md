# Catalog pipeline

This directory owns the catalog ingestion, transformation, migration, and scaffold workflow consumed by code/app/backend/Catalog. It is not an application API project.

## Data flow

~~~text
IGDB/Steam → dlt ingestion → dbt staging → intermediate → marts/bridges → grate → catalogdb → scaffolded C# model
~~~

## Ownership rules

- Complete dlt, dbt, grate, and scaffolding before exposing new Catalog fields or endpoints.
- Data/ is database-first scaffolded output; never edit it manually.
- Catalog is an in-process Backend.API module, not a deployed service.
- Play owns playdb and Notifications owns notificationsdb; do not move those schemas into this directory.
- Catalog API queries use the scaffolded model and db.Games; GamesSearches is search infrastructure, not a Catalog API source.
- Keep transformations in staging → intermediate → marts/bridges order.
- Validate referenced game/company IDs with inner joins.
- Bridges use one target-side FK, deterministic DISTINCT ON, and YAML alias: to remove bridge_.

## Adding catalog data

1. Add the endpoint/resource to pipeline/endpoints.json and dlt.
2. Add staging metadata and dbt models.
3. Add marts, bridge SQL, and YAML contracts.
4. Run dbt and apply grate.
5. Run Invoke-Scaffold.
6. Add the Catalog projection/endpoint and regenerate Kubb clients.

Do not calculate popularity or other derived ranking scores in Catalog API; use stored pipeline values.

## Commands

Run from code/catalog/:

~~~powershell
Invoke-DbtBuild
Invoke-Scaffold
~~~

AppHost provides catalogdb but does not apply catalog schema migrations. Keep pipeline credentials in deployment configuration, never in source control. The root AGENTS.md supplies the shared workflow and approval rules.

