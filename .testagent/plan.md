# CatalogAPI test plan

1. Add `CatalogAPI.Tests` with central package references and a project reference to CatalogAPI.
2. Exercise CatalogAPI routes over HTTP using TUnit's `TestWebApplicationFactory`.
3. Test IGDB popscore and generic game browse reject an inclusive date range.
4. Build the narrow test project and run it through the TUnit/Microsoft Testing Platform runner.
