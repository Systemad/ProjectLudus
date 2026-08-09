# Project Ludus

Game-index is a game discovery platform: IGDB/Steam data flows through the catalog pipeline into PostgreSQL, Backend.API, and the web/mobile clients.

## Repository map

~~~text
code/
├── app/
│   ├── AppHost.cs
│   ├── backend/
│   │   ├── Backend.API/       # public HTTP host and OpenAPI
│   │   ├── Catalog/           # in-process catalog module
│   │   ├── Play/              # users, Steam login, wishlists and lists
│   │   ├── Notifications/     # release-alert worker
│   │   └── backend.slnx
│   ├── frontend/apps/game-index/
│   └── mobile/
└── catalog/                   # dlt, dbt, grate and scaffold inputs
~~~

## Universal rules

- Read this file and the nearest subproject instructions before working.
- Work one Linear issue per local branch and one focused concern per pull request.
- Create branches locally. Keep all work uncommitted and local by default.
- Do not commit, push, open a pull request, create or modify Linear issues, or change Linear state without explicit permission for that specific action.
- Inspect the assigned issue, current status, branch, and only the files in scope.
- Preserve module ownership, generated-file boundaries, and existing route/API contracts.
- Run focused formatting, linting, typechecking, and tests for the changed slice; report blockers instead of broad unrelated cleanup.

## Scoped instructions

- [code/app/AGENTS.md](code/app/AGENTS.md): Aspire, backend composition, clients, and app resources.
- [code/app/mobile/AGENTS.md](code/app/mobile/AGENTS.md): Android Expo architecture and device work.
- [code/catalog/AGENTS.md](code/catalog/AGENTS.md): pipeline, dbt, grate, and scaffold rules.

The scoped file is authoritative for its directory; this file supplies the workflow and repository boundaries.

