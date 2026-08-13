# Pipeline

This directory contains the ingestion and transformation workflows that populate the catalog database and Typesense indexes consumed by Backend.API and the clients. Run commands from `code/catalog/pipeline`; local secrets stay in the deployment environment and are never committed.

## Dependencies

```
igdb_default ──→ popscores ──→ dbt_build_igdb
                    │
                    └──→ tracked_games ──→ steam CCU & steam_store
```

- **igdb flow** must run at least once before steam flows work (populates game catalog + Steam ID mappings)
- **popscores** needed for popularity-based tracking (5 of 6 tracking categories use it)
- **steam CCU** and **steam_store** can run anytime after igdb_default + popscores have populated the DB
- Within the **igdb flow**, order is: igdb_default → popscores → dbt_build_igdb → dbt_build_steam (all sequential)

## Setup (one-time)

```bash
cd /opt/pipeline
rm -rf .venv && uv sync --no-dev
export PREFECT_API_URL=http://prefect-server:4200/api

uv run prefect gcl create pipeline-lock --limit 1
#uv run prefect work-pool create pipeline-pool
uv run prefect deploy --all
```

## Update affected batch flows

```bash
cd /opt/pipeline && git pull && uv sync --no-dev
uv run prefect deploy -n analytics-hourly -n steam-store -n steam-game-index
```

Catalog schema changes flow through dlt/dbt and the database migration/scaffold process before backend code changes. Keep user data and release-alert scheduling in the application modules under `code/app/backend`; this pipeline is for catalog and search data.
