# Application workspace

The application workspace contains Aspire orchestration, the modular backend, the web client, and the Android mobile client.

## Architecture

~~~text
AppHost
├── Backend.API       public HTTP host, OpenAPI and Scalar
│   ├── Catalog       database-first catalog read module
│   └── Play          code-first users and lists module
├── Notifications     code-first TickerQ worker; no HTTP endpoints
├── catalogdb / playdb / notificationsdb
├── mobile-api         device tunnel to Backend.API
└── frontend           explicitly started Vite resource
~~~

Backend.API is the only public API. Cross-module responses are composed there. Persistence stays in its owning module. Catalog reads code/catalog/Data; Play and Notifications own their EF Core migrations.

## Backend rules

- Keep Catalog and Play features in their owning Features folders and module extensions.
- Catalog queries start from db.Games, use no-tracking projections, stored ordering, and FirstOrDefaultAsync for ordinary lookups.
- Do not use GamesSearches in Catalog API paths or calculate popularity in the API.
- Do not edit scaffolded catalog models or generated clients.

## Aspire and clients

Use the Aspire skill and Aspire MCP for AppHost work. AppHost owns resource references, service discovery, device tunnels, and Play/Notifications migration application; migrations create tables, not databases. Development OpenAPI is exposed at http://localhost:5141/openapi/v1.json.

From this directory:

~~~powershell
aspire start
aspire stop
~~~

From backend/ with Aspire stopped:

~~~powershell
dotnet build backend.slnx
~~~

After an OpenAPI change, regenerate Kubb clients from the running API. Use generated hooks and types only; never hand-edit src/gen/.

Web-specific guidance is in frontend/README.md. Android-specific guidance is in mobile/AGENTS.md. The universal workflow in the root AGENTS.md applies here.

