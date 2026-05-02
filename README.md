# ProjectLudus

Source code for game-index.app, a website to browse data from IGDB.

## Project Structure

```
code/
├── frontend/          # React monorepo (Vite+, pnpm)
│   ├── apps/game-index/   # Main React app
│   │   ├── src/routes/    # Tanstack Router (file-based)
│   │   ├── src/gen/       # Auto-generated Tanstack Query hooks
│   │   └── src/features/  # Feature-based modules
│   └── packages/
│       └── ui/            # YamadaUI component library
│
├── backend/           # .NET solution
│   ├── CatalogAPI/    # Read-only API (Entity Framework, PostgreSQL)
│   ├── PlayAPI/       # User interactions API
│   └── ServiceDefaults/   # Shared .NET configuration
│
└── orchestration/     # Data pipelines
    ├── dbt/           # dbt-core transformations
    ├── DLT/           # Data ingestion
    └── dagster/       # Pipeline orchestration
```

## How It Works

### Frontend (`code/frontend`)
- **Framework**: React with Vite+
- **Routing**: Tanstack Router (file-based, auto-generated routeTree.gen.ts)
- **State/API**: Tanstack Query with auto-generated hooks from `src/gen`
- **UI Components**: YamadaUI (via `packages/ui`)
- **Search**: Typesense with `react-instantsearch`
- **Package Manager**: pnpm (wrapped by Vite+ via `vp` CLI)

### Backend (`code/backend`)
- **CatalogAPI**: Read-only API serving game catalog data (Entity Framework, PostgreSQL)
- **PlayAPI**: API for storing user interactions (consent, analytics events)
- **ServiceDefaults**: Shared .NET configuration and middleware

### Data Orchestration (`code/orchestration`)
- **Dagster**: Orchestration tool for data pipelines
- **DLT**: Data ingestion
- **dbt-core**: Data transformation

## Stack

| Layer | Technology |
|-------|------------|
| Frontend | React, Tanstack Router, Tanstack Query, YamadaUI, Typesense, Vite+ |
| Backend | ASP.NET, .NET Aspire, Entity Framework |
| Database | PostgreSQL |
| Orchestration | Dagster, DLT, dbt-core |

