using CatalogAPI.Features;
using Parquet.Serialization;
using Shared.Features;
using Shared.Features.IGDB;
using Shared.Features.References.Genre;
using Shared.Features.References.Platform;
using Shared.Features.References.Theme;

namespace CatalogAPI.Seeding;

public class SeedingService(IgdbService igdbService) : ISeederService
{
    public async Task BeginSeedAsync(CancellationToken cancellationToken)
    {
        await SeedAsync<IgdbGame>(IgdbReference.GAME, cancellationToken);

        //await SeedAsync<Genre>(IgdbReference.GENRE, cancellationToken);
        //await SeedAsync<Company>(IgdbReference.COMPANY, cancellationToken);
        //await SeedAsync<Theme>(IgdbReference.THEME, cancellationToken);
        //await SeedAsync<Platform>(IgdbReference.PLATFORM, cancellationToken);
        //await SeedAsync<GameEngine>(IgdbReference.GAME_ENGINE, cancellationToken);
    }

    public async Task SeedAsync<T>(IgdbReference reference, CancellationToken cancellationToken)
        where T : IgdbResponse
    {
        long amount = 0;
        await foreach (
            var page in igdbService
                .FetchAllPagesAsync<T>(reference)
                .WithCancellation(cancellationToken)
        )
        {
            var append = amount > 0;
            var file = $"data/{reference}.parquet";
            await ParquetSerializer.SerializeAsync(
                page.Items,
                file,
                new ParquetSerializerOptions { Append = append },
                cancellationToken: cancellationToken
            );
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
                break;
        }
    }
}
