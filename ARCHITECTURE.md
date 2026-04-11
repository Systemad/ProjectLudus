# ProjectLudus Architecture Overview

## Project Structure

```
ProjectLudus/
├── code/
│   ├── frontend/          # React monorepo (Vite+, pnpm)
│   │   ├── apps/game-index/   # Main React app
│   │   │   ├── src/routes/    # Tanstack Router (file-based)
│   │   │   └── src/gen/       # Auto-generated Tanstack Query hooks
│   │   └── packages/
│   │       └── ui/            # YamadaUI component library
│   │
│   ├── backend/           # .NET monorepo
│   │   ├── AppHost/       # ASP.NET Aspire (orchestration)
│   │   ├── PlayAPI/       # ASP.NET - stores user interactions (EFCore)
│   │   ├── CatalogAPI/    # ASP.NET - read-only API (EFCore, scaffolded from Postgres)
│   │   └── ServiceDefaults/   # Shared .NET configuration
│   │
│   └── orchestration/     # Data pipeline
│       ├── src/           # DLT pipelines (Python)
│       ├── dbt/           # dbt-core transformations
│       └── dagster/       # Dagster orchestration
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
- **Orchestration**: ASP.NET Aspire (AppHost)
- **PlayAPI**: ASP.NET API for storing user interaction data (EFCore)
- **CatalogAPI**: ASP.NET read-only API serving catalog data to frontend (EFCore, scaffolded from PostgreSQL `catalogdev` database)
- **ServiceDefaults**: Shared .NET configuration and middleware

### Data Orchestration (`code/orchestration`)
- **Dagster**: Orchestration tool for data pipelines
- **DLT**: Data ingestion (Python)
- **dbt-core**: Data transformation

## Data Flow

1. External data sources → DLT (ingestion)
2. DLT → Dagster (orchestration)
3. Dagster → dbt (transformation)
4. Transformed data → PostgreSQL
5. PostgreSQL → CatalogAPI (ASP.NET)
6. CatalogAPI → Frontend (React)

## Development Commands

| Component | Command | Description |
|-----------|---------|-------------|
| Frontend | `vp dev` | Start Vite+ dev server |
| Frontend | `vp check` | Run lint, format, type checks |
| Frontend | `vp test` | Run tests |
| Backend | Run `AppHost` project | Start ASP.NET Aspire |
| Orchestration | `dg dev` | Start Dagster UI |

## Technology Stack

| Layer | Technology |
|-------|------------|
| Frontend | React, Vite+, Tanstack Router, Tanstack Query, YamadaUI, Typesense |
| Backend | ASP.NET, ASP.NET Aspire, EFCore |
| Database | PostgreSQL |
| Orchestration | Dagster, DLT, dbt-core |