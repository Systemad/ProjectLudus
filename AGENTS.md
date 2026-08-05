# Project Ludus

Game discovery platform: IGDB/Steam → dlt/dbt → PostgreSQL → Backend.API → web and mobile clients.

## Repository map

```text
code/app/
├── AppHost.cs                 # Aspire orchestration, migrations, tunnels
├── backend/
│   ├── Backend.API/           # Only HTTP host/OpenAPI document
│   ├── Catalog/               # In-process catalog read module
│   ├── Play/                  # Steam auth, users, wishlists, lists
│   ├── Notifications/         # TickerQ worker; no HTTP endpoints
│   ├── ServiceDefaults/
│   ├── Tests/
│   └── backend.slnx
├── frontend/apps/game-index/  # Vite, TanStack, Astryx XDS
└── mobile/                    # Expo Router, Expo UI, React Native
code/catalog/                  # dlt, dbt, grate, catalog scaffold
```

`Catalog`, `Play`, and `Notifications` are projects. `Modules`, `Workers`, and `Tests` are solution folders.

## Ownership

- `Backend.API` composes modules and maps the public API.
- `Catalog` reads the database-first model from `code/catalog/Data`; never edit scaffolded files.
- `Play` owns code-first `playdb`, Steam login, users, wishlists, and lists.
- `Notifications` owns code-first `notificationsdb` and scheduled release alerts.
- Keep persistence in its module; compose Catalog + Play only in `Backend.API`.
- Catalog API queries use `db.Games`, not `GamesSearches`, and do not calculate popularity scores.

## Commands

```powershell
# code/app/
aspire start
aspire stop

# code/app/backend/ (Aspire stopped)
dotnet build backend.slnx

# web
pnpm exec kubb generate
tsc -b; vp build

# code/app/mobile/
pnpm run generate
pnpm run fmt
pnpm exec tsc --noEmit
pnpm run lint

# code/catalog/
Invoke-DbtBuild
Invoke-Scaffold
```

## Data flow

```text
Catalog: pipeline → grate → scaffold → Catalog → Backend.API → Kubb
Play:    EF migration → Play → Backend.API → Kubb
Alerts:  catalog + wishlist data → Notifications → notificationsdb
```

Catalog schema changes go through dbt/grate/scaffold before API work. After API changes, start Aspire and regenerate affected clients.

## Aspire, clients, and devices

- Use `.agents/skills/aspire/SKILL.md` and Aspire MCP for AppHost work; do not run ad-hoc Docker/dotnet against managed resources.
- AppHost declares `catalogdb`, `playdb`, `notificationsdb`, `mobile-api`, and frontend. Local values belong in `code/app/appsettings.Development.json`.
- AppHost applies Play/Notifications migrations; migrations create tables, not databases.
- Development OpenAPI: `http://localhost:5141/openapi/v1.json`.
- Use generated Kubb hooks only; never edit `src/gen/` or handwrite clients.
- Physical devices use the current `mobile-api` tunnel URL as `EXPO_PUBLIC_API_URL` in `code/app/mobile/.env`.
- Start mobile dev client from `code/app/mobile/`:

  ```powershell
  $env:EXPO_UNSTABLE_MCP_SERVER = "1"; pnpm expo start --dev-client
  ```

- Enable the local `agent-device` MCP in Codex `/mcp` for device automation. If MCP is unavailable, use the CLI after `agent-device --version` and `agent-device help workflow`.
- Run `pnpm run fmt` after every mobile code change. Do not launch clients for generation or typechecking alone.

## Subproject instructions

- [`code/app/AGENTS.md`](code/app/AGENTS.md): AppHost, backend, and clients
- [`code/app/mobile/AGENTS.md`](code/app/mobile/AGENTS.md): Expo architecture and device work
- [`code/catalog/AGENTS.md`](code/catalog/AGENTS.md): pipeline and scaffold rules
