using NodaTime;
using Npgsql;
using NpgsqlTypes;

namespace Catalog.Worker.Services.Seed;

public class CompanySeeder(NpgsqlConnection connection)
{
    public async Task SeedCompaniesAsync(List<IGDB.Models.Company> companies)
    {
        await connection.OpenAsync();

        // TODO: search how do handle NULLABLE, (maybe exlude if its null)
        await using var writer = connection.BeginBinaryImport(
            "COPY Companies (Id, Name, Slug, UpdatedAt, CreatedAt, StartDate, Url, Metadata) FROM STDIN (FORMAT BINARY)"
        );
        foreach (var company in companies)
        {
            writer.StartRow();
            writer.Write(company.Id, NpgsqlDbType.Text);
            writer.Write(company.Name, NpgsqlDbType.Text);
            writer.Write(company.Slug, NpgsqlDbType.Text);
            writer.Write(company.UpdatedAt, NpgsqlDbType.TimestampTz);
            writer.Write(company.CreatedAt, NpgsqlDbType.TimestampTz);
            writer.Write(company.StartDate, NpgsqlDbType.TimestampTz);
            writer.Write(company.Url, NpgsqlDbType.Text);
            writer.Write(company, NpgsqlDbType.Jsonb);
        }

        await writer.CompleteAsync();
    }

    public async Task SeedFranchisesAsync(List<IGDB.Models.Franchise> franchises)
    {
        await connection.OpenAsync();

        using (
            var writer = connection.BeginBinaryImport(
                "COPY Franchises (Id, Name, Slug, Url, UpdatedAt, CreatedAt, Metadata) FROM STDIN (FORMAT BINARY)"
            )
        )
        {
            foreach (var franchise in franchises)
            {
                writer.StartRow();
                writer.Write(franchise.Id, NpgsqlDbType.Bigint);
                writer.Write(franchise.Name, NpgsqlDbType.Text);
                writer.Write(franchise.Slug, NpgsqlDbType.Text);
                writer.Write(franchise.Url, NpgsqlDbType.Text);
                writer.Write(franchise.UpdatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(franchise.CreatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(franchise, NpgsqlDbType.Jsonb);
            }

            writer.Complete();
        }
    }

    public async Task SeedGameEngines(List<IGDB.Models.GameEngine> gameEngines)
    {
        await connection.OpenAsync();

        // TODO: Add Created At!
        using (
            var writer = connection.BeginBinaryImport(
                "COPY GameEngines (Id, Name, Slug, Url, Metadata, Logo, UpdatedAt, CreatedAt) FROM STDIN (FORMAT BINARY)"
            )
        )
        {
            foreach (var gameEngine in gameEngines)
            {
                writer.StartRow();
                writer.Write(gameEngine.Id, NpgsqlDbType.Bigint);
                writer.Write(gameEngine.Name, NpgsqlDbType.Text);
                writer.Write(gameEngine.Slug, NpgsqlDbType.Text);
                writer.Write(gameEngine.Url, NpgsqlDbType.Text);
                writer.Write(gameEngine, NpgsqlDbType.Jsonb);
                writer.Write(gameEngine.Logo, NpgsqlDbType.Jsonb);
                writer.Write(gameEngine.UpdatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(gameEngine.CreatedAt, NpgsqlDbType.TimestampTz);
            }

            writer.Complete();
        }
    }
}
