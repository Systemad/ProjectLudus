# AppHost, Backend, and Clients

Use `.agents/skills/aspire/SKILL.md` and Aspire MCP for AppHost work. Do not operate AppHost-managed resources with ad-hoc Docker commands.

## Runtime

```text
AppHost
├── Backend.API       public HTTP host, Scalar, OpenAPI
│   ├── Catalog       in-process read module → catalogdb
│   ├── Play          in-process user module → playdb
│   └── UserLibrary   host-level composition
├── Notifications     TickerQ worker → catalogdb + notificationsdb
├── Play/Notifications migration resources
├── mobile-api        device tunnel to Backend.API
└── frontend          explicitly started Vite resource
```

`Backend.API` is the only public API. Catalog and Play are class-library modules; Notifications has no public endpoint.

## Backend rules

- Put Catalog/Play features in their owning `Features` folders and module extensions.
- Compose cross-module responses only in `Backend.API`.
- Keep scheduled work in Notifications.
- Catalog uses scaffolded `code/catalog/Data`; Play and Notifications own code-first EF contexts/migrations.
- Use `db.Games`-rooted projections, no-tracking reads, stored ordering, and `FirstOrDefaultAsync` for ordinary lookups.
- Do not use `GamesSearches` in Catalog API paths or calculate popularity in the API.

## AppHost resources

| Resource | Owner |
|---|---|
| `catalogdb` | Catalog; Notifications read path |
| `playdb` | Play |
| `notificationsdb` | Notifications |

Connection values belong in `code/app/appsettings.Development.json`. AppHost injects them with references, runs Play/Notifications migrations, supplies `Steam__ApiKey`, and exposes `mobile-api`. Migrations create tables; provision databases first.

## Build and clients

```powershell
# code/app/
aspire start

# code/app/backend/ (Aspire stopped)
dotnet build backend.slnx

# web/frontend
pnpm exec kubb generate

# mobile/
pnpm run generate
pnpm run fmt
```

Kubb reads `http://localhost:5141/openapi/v1.json`; keep Development HTTP available. Never edit generated clients. Tests use TUnit, generated clients, and Testcontainers.

Web uses Kubb/TanStack/Astryx XDS with StyleX, not Tailwind. Mobile uses generated hooks, `String(id)` for generated IDs, and no BigInt query keys.

## Mobile runtime

Set the AppHost `mobile-api` URL in `mobile/.env` as `EXPO_PUBLIC_API_URL`, then from `code/app/mobile/` run:

```powershell
$env:EXPO_UNSTABLE_MCP_SERVER = "1"; pnpm expo start --dev-client
```

Enable `agent-device` MCP in Codex `/mcp` for device QA. For CLI fallback, first run `agent-device --version` and `agent-device help workflow`. Run `pnpm run fmt` after every mobile change; do not launch clients for generation or typechecking alone.
