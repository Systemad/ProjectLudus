using CatalogAPI.Seeding;
using IGDB.Models;
using Parquet.Serialization;
using BuildingBlocks.Features;

namespace Catalog.Worker.Services;

public class DataFetcherService(IgdbService service) : IDataFetcherService
{
    public async Task FetchDataAsync(CancellationToken cancellationToken)
    {
        //await SeedAsync<Game>(IgdbType.GAME, cancellationToken);

        //await SeedAsync<Genre>(IgdbType.GENRE, cancellationToken);
        await FetchDataAsync<Company>(IgdbType.COMPANY, cancellationToken);
        //await SeedAsync<Theme>(IgdbType.THEME, cancellationToken);
        //await SeedAsync<Platform>(IgdbType.PLATFORM, cancellationToken);
        //await SeedAsync<GameEngine>(IgdbType.GAME_ENGINE, cancellationToken);
    }

    public Task<bool> ParquetExistsAsync(CancellationToken cancellationToken)
    {
        bool exist = false;
        if (true)
        {
            // check if all files exist!
            exist = true;
        }

        exist = false;
        return Task.FromResult(exist);
    }

    public async Task FetchDataAsync<T>(
        IgdbType type,
        CancellationToken cancellationToken,
        bool overrideFile = false
    )
    {
        long amount = 0;
        await foreach (
            var page in service.FetchAllPagesAsync<T>(type).WithCancellation(cancellationToken)
        )
        {
            var append = amount > 0;
            var file = $"Data/{type}.parquet";
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
