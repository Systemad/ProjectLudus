using CatalogAPI.Features;
using IGDB.Models;
using Parquet.Serialization;
using Shared.Features;

namespace CatalogAPI.Seeding;

public class SeedingService(IgdbService service) : ISeederService
{
    public async Task BeginSeedAsync(CancellationToken cancellationToken)
    {
        await SeedAsync<Game>(IgdbType.GAME, cancellationToken);

        //await SeedAsync<Genre>(IgdbReference.GENRE, cancellationToken);
        //await SeedAsync<Company>(IgdbReference.COMPANY, cancellationToken);
        //await SeedAsync<Theme>(IgdbReference.THEME, cancellationToken);
        //await SeedAsync<Platform>(IgdbReference.PLATFORM, cancellationToken);
        //await SeedAsync<GameEngine>(IgdbReference.GAME_ENGINE, cancellationToken);
    }

    public async Task SeedAsync<T>(IgdbType type, CancellationToken cancellationToken)
    {
        long amount = 0;
        await foreach (
            var page in service
                .FetchAllPagesAsync<T>(type)
                .WithCancellation(cancellationToken)
        )
        {
            var append = amount > 0;
            var file = $"data/{type}.parquet";
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