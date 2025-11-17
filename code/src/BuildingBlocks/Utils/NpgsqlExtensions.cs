using Npgsql;
using NpgsqlTypes;

namespace BuildingBlocks.Utils;

public static class NpgsqlExtensions
{
    public static void WriteValueOrNull<T>(this NpgsqlBinaryImporter writer, T? value, NpgsqlDbType dbType)
        where T : struct
    {
        if (value.HasValue)
        {
            writer.Write(value.Value, dbType);
        }
        else
        {
            writer.WriteNull();
        }
    }
    
    public static void WriteValueOrNull<T>(this NpgsqlBinaryImporter writer, T? value, NpgsqlDbType dbType)
        where T : notnull
    {
        if (value is null)
        {
            writer.WriteNull();
        }
        else
        {
            writer.Write(value, dbType);
        }
    }
}