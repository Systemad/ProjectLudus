using Backend.API.Tests.Setup;
using CatalogAPISDK.Catalog.Games.Browse;
using CatalogAPISDK.Catalog.Igdb.Popscore;
using Microsoft.Kiota.Abstractions;

namespace Backend.API.Tests.Features;

public class CatalogApiContractTests
{
    [ClassDataSource<CatalogApiWebApplicationFactory>(Shared = SharedType.PerTestSession)]
    public CatalogApiWebApplicationFactory Factory { get; init; } = null!;

    [Test]
    public async Task Popscore_InvertedDateRange_ReturnsValidationProblem()
    {
        using var httpClient = Factory.CreateClient();
        var apiClient = CatalogApiWebApplicationFactory.CreateApiClient(httpClient);
        ApiException? exception = null;

        try
        {
            await apiClient.Catalog.Igdb.Popscore.GetAsync(config =>
            {
                config.QueryParameters =
                    new PopscoreRequestBuilder.PopscoreRequestBuilderGetQueryParameters
                    {
                        From = new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero),
                        To = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    };
            });
        }
        catch (ApiException caught)
        {
            exception = caught;
        }

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception!.ResponseStatusCode).IsEqualTo(400);
    }

    [Test]
    public async Task Browse_InvertedDateRange_ReturnsValidationProblem()
    {
        using var httpClient = Factory.CreateClient();
        var apiClient = CatalogApiWebApplicationFactory.CreateApiClient(httpClient);
        ApiException? exception = null;

        try
        {
            await apiClient.Catalog.Games.Browse.GetAsync(config =>
            {
                config.QueryParameters =
                    new BrowseRequestBuilder.BrowseRequestBuilderGetQueryParameters
                    {
                        From = new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero),
                        To = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    };
            });
        }
        catch (ApiException caught)
        {
            exception = caught;
        }

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception!.ResponseStatusCode).IsEqualTo(400);
    }
}
