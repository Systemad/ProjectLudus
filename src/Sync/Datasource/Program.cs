using Npgsql;
using NpgsqlTypes;
using Shared.Features;

var connectionString = "Host=localhost;Port=5433;Username=myuser;Password=mypassword;Database=mydatabase";

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.EnableDynamicJson();

await using var dataSource = dataSourceBuilder.Build();
await using var connection = await dataSource.OpenConnectionAsync();

var gObvj = new IGDBGame
{
    CreatedAt = 0,
    Name = null,
    Checksum = null
};
var obj = new GameEntity
{
    Id = 0,
    Name = "null",
    GameType = 0,
    Platforms = [],
    GameEngines = [],
    Genres = [],
    Themes = [],
    Rating = 0,
    RatingCount = 0,
    TotalRating = 0,
    TotalRatingCount = 0,
    RawData = gObvj
};

await using var command1 = new NpgsqlCommand("INSERT INTO test (data) VALUES ($1)", connection)
{
    Parameters = { new() { Value = obj, NpgsqlDbType = NpgsqlDbType.Jsonb } }
};
await command1.ExecuteNonQueryAsync();

await using var command2 = new NpgsqlCommand("SELECT data FROM test", connection);
await using var reader = await command2.ExecuteReaderAsync();
while (await reader.ReadAsync())
{
    var myPoco2 = reader.GetFieldValue<GameEntity>(0);
    Console.WriteLine(myPoco2.Id);
}

/*
await using var command = new NpgsqlCommand("SELECT * FROM paradedb. version_info();", connection);
await using var reader = await command.ExecuteReaderAsync();

if (await reader.ReadAsync())
{
    var result = reader.GetString(0);
    Console.WriteLine(result);
}
*/  

//await command.ExecuteNonQueryAsync();