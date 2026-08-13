# .NET 11 Preview 7 backend upgrade plan

## Baseline

- SDK: `11.0.100-preview.7.26381.103`.
- Projects: `net10.0`; ASP.NET Core and EF Core packages remain on 10.x.
- Compatibility targets: Npgsql, TimescaleDB, Aspire, TickerQ, TUnit, Kiota, and OpenAPI packages.

## Upgrade sequence

1. Build and test with the pinned SDK; fix generated-client build ordering first.
2. Benchmark MSBuild Server and experimental `dotnet build -mt`.
3. Add MTP CI policies: `--timeout`, `--maximum-failed-tests`, and optionally `--list-tests json`.
4. Build a .NET 11 package/provider compatibility matrix.
5. Retarget projects and packages together; validate migrations, SQL, Aspire, containers, and generated clients.
6. Compare build, API, query, and startup performance against the .NET 10 baseline.

## Candidate improvements

- OpenAPI 3.2 for future SSE or newer schema requirements.
- HybridCache for shared catalog DTO/computation caching; retain OutputCache for response caching.
- EF Core 11 query translations for natural grouped-navigation or grouped-first queries; verify SQL with `EXPLAIN`.
- EF migration bundles with Central Package Management.
- Validation localization if localized API errors become required.
- Request compression or connection eviction only for demonstrated HTTP bottlenecks.

## Constraints

- No preview-only API adoption without a concrete use case and benchmark.
- Preserve Catalog, Play, and Notifications persistence ownership.
- Review OpenAPI and generated-client diffs explicitly.
- Do not modify production databases or deployment configuration during evaluation.
