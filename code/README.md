# ProjectLudus

Source code for game-index.app, a website to browse data from IGDB

## Project Structure

```
code/
├── frontend/      # React + Vite+ monorepo
│   ├── apps/game-index/   # Main React app
│   └── packages/         # UI components (YamadaUI) + utils
├── backend/       # .NET monorepo
│   ├── AppHost/         # .NET Aspire
│   ├── PlayAPI/        # User interactions (Entity Framework)
│   └── CatalogAPI/    # Read-only API (Entity Framework, PostgreSQL)
└── orchestration/ # Data pipelines (Dagster, DLT, dbt)
```

## Data Flow

External → DLT → PostgreSQL → dbt → CatalogAPI → Frontend

## Stack

| Layer | Technology |
|-------|------------|
| Frontend | React, Tanstack Router, Tanstack Query, YamadaUI, Typesense, Vite+ |
| Backend | ASP.NET, .NET Aspire, Entity Framework |
| Database | PostgreSQL |
| Orchestration | Dagster, DLT, dbt-core |

## Key Features

- **Search**: Typesense-powered search with faceted filtering
- **Game Catalog**: IGDB-sourced game data with full metadata
- **Data Pipeline**: Automated ETL from IGDB → PostgreSQL → Typesense

## Potential Todo / Features

- Personalized recommendations
- Discovery features (trending, similar games, curated lists)
- More search filters and sorting options