using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using Shared.Features.Games;
using SyncService.Data;

namespace SyncService.Features.Company;

public class CompanyDatabaseService(SyncDbContext context)
{
    public async Task InsertCompaniesAsync(List<Shared.Features.Games.Company> companies)
    {
        var entities = companies.Select(company => new CompanyEntity
        {
            Id = company.Id,
            Description = company.Description,
            Name = company.Name,
            ImageId = company.Logo.ImageId,
            RawData = company
        });
        
        var dedupedCompanies = entities
            .GroupBy(g => g.Id)
            .Select(g => g.OrderByDescending(x => x.RawData.UpdatedAt).First())
            .ToList();
        
        await context.Companies.ExecuteBulkInsertAsync(dedupedCompanies, options =>
        {
            options.BatchSize = 10_000;
            options.OnProgress = rowsCopied => { Console.WriteLine($"Copied {rowsCopied} rows so far..."); };
        }, onConflict: new OnConflictOptions<CompanyEntity>
        {
            Match = e => new
            {
                e.Id,
            },

            Update = (inserted, excluded) => inserted,
            Where = (inserted, excluded) => excluded.RawData.UpdatedAt > inserted.RawData.UpdatedAt,
        });
    }
}