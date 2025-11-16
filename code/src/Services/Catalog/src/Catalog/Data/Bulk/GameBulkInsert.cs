using IGDB.Models;
using Npgsql;
using NpgsqlTypes;

namespace Catalog.Data.Bulk;

public class GameBulkInsert : IBulkInsert<Game>
{
    public string CopyCommand =>
        "COPY companies (id, name, slug, game_type, rating, rating_count, total_rating, total_rating_count, url, first_release_date, updated_at, created_at, metadata) FROM STDIN (FORMAT BINARY)";

    public void WriteRow(NpgsqlBinaryImporter writer, Game item)
    {
        writer.Write(item.Id, NpgsqlDbType.Bigint);
        writer.Write(item.Name, NpgsqlDbType.Text);
        writer.Write(item.Slug, NpgsqlDbType.Text);
        // maybe just int?
        writer.Write(item.GameType.Value.Id, NpgsqlDbType.Bigint);
        writer.Write(item.Rating, NpgsqlDbType.Double);
        writer.Write(item.RatingCount, NpgsqlDbType.Bigint);
        writer.Write(item.TotalRating, NpgsqlDbType.Double);
        writer.Write(item.TotalRatingCount, NpgsqlDbType.Bigint);
        writer.Write(item.Url, NpgsqlDbType.Text);
        writer.Write(item.FirstReleaseDate, NpgsqlDbType.TimestampTz);
        writer.Write(item.UpdatedAt, NpgsqlDbType.TimestampTz);
        writer.Write(item.CreatedAt, NpgsqlDbType.TimestampTz);
        writer.Write(item, NpgsqlDbType.Jsonb);
    }
}
