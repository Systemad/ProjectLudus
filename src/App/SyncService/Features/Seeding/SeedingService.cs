using System.Text.Json;
using Parquet.Serialization;
using Shared.Features;
using Shared.Features.IGDB;

namespace SyncService.Features.Seeding;

public class SeedingService(IgdbService igdbService) : ISeederService
{
    public static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };

    public async Task BeginSeedAsync(CancellationToken cancellationToken)
    {
    }

    public async Task SeedAllAsync<T>(CancellationToken cancellationToken, IgdbReference reference)
        where T : IgdbResponse
    {
        long amount = 0;
        await foreach (var page in igdbService.FetchAllPagesAsync<T>(reference).WithCancellation(cancellationToken))
        {
            var append = amount > 0;
            var file = $"data/{reference}.parquet";
            await ParquetSerializer.SerializeAsync(page.Items, file, new ParquetSerializerOptions { Append = append },
                cancellationToken: cancellationToken);
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
                break;
        }
    }

    public async Task SeedGenresAsync(CancellationToken cancellationToken)
    {
        long amount = 0;
        await foreach (var page in igdbService.FetchAllPagesAsync<Genre>(IgdbReference.GENRE))
        {
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
            {
                break;
            }

            var dedouped = page.Items.Dedoup();
            // TODO: INSERT INTO JSON OR PARQUET
        }
    }

    public async Task SeedGamesAsync(CancellationToken cancellationToken)
    {
        long amount = 0;
        await foreach (var page in igdbService.FetchAllPagesAsync<IgdbGame>(IgdbReference.GAME))
        {
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
            {
                break;
            }

            var dedouped = page.Items.Dedoup();
            // TODO: INSERT INTO JSON OR PARQUET
        }
    }

    public async Task SeedCompaniesAsync(CancellationToken cancellationToken)
    {
        long amount = 0;
        await foreach (var page in igdbService.FetchAllPagesAsync<Company>(IgdbReference.COMPANY))
        {
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
            {
                break;
            }

            var dedouped = page.Items.Dedoup();
            // TODO: INSERT INTO JSON OR PARQUET
        }
    }
    /*
    public async Task SeedGamesAsync()
    {
        long amount = 0;
        await foreach (var page in igdbService.GetAllGamesAsync())
        {
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
            {
                break;
            }

            //await ProcessCompanyEntitiesAsync(page.Items);
        }
    }

    public async Task SeedCompaniesAsync()
    {
        long amount = 0;
        await foreach (var page in igdbService.GetAllCompaniesAsync())
        {
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
            {
                break;
            }

            var dedupedCompanies = page.Items
                .GroupBy(g => g.Id)
                .Select(g => g.OrderByDescending(x => x.UpdatedAt).First())
                .ToList();

            var games = dedupedCompanies.Select(x => new RawCompanyEntity
            {
                Id = x.Id,
                Payload = x
            });

            // TODO: USE BULKINSERT
            //await ProcessCompanyEntitiesAsync(page.Items);
        }
    }
    */
}