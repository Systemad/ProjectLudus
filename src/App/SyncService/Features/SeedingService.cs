using Shared.Features;
using Shared.Features.IGDB;
using SyncService.Data;
using SyncService.Data.Entities;
using SyncService.Data.Features.Companies;
using SyncService.Features.Games;

namespace SyncService.Features;

public class SeedingService(SyncDbContext context, IgdbService igdbService) : ISeederService
{

    public async Task SeedGamesAsync()
    {
        long amount = 0;
        await foreach (var page in igdbService.GenericAsyncV1<IgdbGame>(IgdbReference.GAMES))
        {
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
            {
                break;
            }
            
            var dedupedGames = page.Items
                .GroupBy(g => g.Id)
                .Select(g => g.OrderByDescending(x => x.UpdatedAt).First())
                .ToList();
            
            var games = dedupedGames.Select(x => new RawGameEntity
            {
                Id = x.Id,
                Payload = x
            });
        }
    }
    public async Task SeedCompaniesAsync()
    {
        long amount = 0;
        await foreach (var page in igdbService.GenericAsyncV1<Company>(IgdbReference.COMPANIES))
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