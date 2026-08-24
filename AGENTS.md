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
- When multiple agents work in parallel, state file ownership and boundaries up front to avoid collisions.
- For non-trivial UI, layout, or copy changes, create several distinct static mocks before editing production components.
- Never access or modify production systems, including live databases or VPS infrastructure, unless explicitly asked for that specific action.
- When a commit is explicitly authorized, follow [Conventional Commits 1.0.0](https://www.conventionalcommits.org/en/v1.0.0/).
- Keep solutions simple: follow YAGNI and avoid speculative abstractions unless told otherwise.
- Treat type safety as a default requirement and use the type system to prevent invalid states.
- Suggest bold ideas when they could materially benefit the work, and state their tradeoffs clearly.
- Inspect the assigned issue, current status, branch, and only the files in scope.
- Preserve module ownership, generated-file boundaries, and existing route/API contracts.
- Run focused formatting, linting, typechecking, and tests for the changed slice; report blockers instead of broad unrelated cleanup.

## Product scope preference

- The mobile app is the default product scope for UI requests. Do not modify the web client unless the user explicitly asks for the web version.
- Bun is used for mobile work.

## Scoped instructions

- [code/app/AGENTS.md](code/app/AGENTS.md): Aspire, backend composition, clients, and app resources.
- [code/app/mobile/AGENTS.md](code/app/mobile/AGENTS.md): Android Expo architecture and device work.
- [code/catalog/AGENTS.md](code/catalog/AGENTS.md): pipeline, dbt, grate, and scaffold rules.

The scoped file is authoritative for its directory; this file supplies the workflow and repository boundaries.

