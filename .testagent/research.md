# CatalogAPI test research

- Framework: .NET 10 with TUnit 1.59 and Microsoft Testing Platform.
- Existing convention: `PlayAPI.Tests` uses an SDK-style project, central package management, implicit usings, nullable enabled, and direct project references.
- CatalogAPI exposes minimal API handlers under `CatalogAPI/Features`.
- The most deterministic HTTP contracts are the date-range validation branches in IGDB popscore and game browse filtering.
- Database-backed integration tests would require Docker and a populated catalog schema. The initial suite uses `TestWebApplicationFactory` and routes that short-circuit before database access.
- The test project includes a Kiota generation target matching PlayAPI. CatalogAPI's OpenAPI document is runtime-generated, so the target activates when `docs/openapi/CatalogAPI.json` is checked in.
- `find-untested-sources` was attempted once, but its required `tree-sitter-language-pack` dependency is not installed in this environment.
