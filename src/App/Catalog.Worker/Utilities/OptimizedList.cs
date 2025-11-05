using System.Buffers;
using System.IO.Pipelines;
using System.Text.Json;

namespace CatalogAPI.Utilities;

// https://danielwoodward.dev/posts/csharp/how-to-efficiently-read-ndjson-in-dotnet-with-pipes
public static class OptimizedList
{
    public static async Task WriteToCacheFileAsync<T>(
        IEnumerable<T> items,
        string filePath,
        bool append = true
    )
    {
        await using var stream = new StreamWriter(filePath, append: append);
        var options = new JsonSerializerOptions { WriteIndented = false };
        foreach (var json in items.Select(entity => JsonSerializer.Serialize(entity, options)))
        {
            await stream.WriteLineAsync(json);
        }
    }

    public static async Task<List<T>> ReadFromStreamAsync<T>(
        Stream stream,
        CancellationToken cancellationToken = default
    )
    {
        var reader = PipeReader.Create(stream);

        var containers = new List<T>();
        while (true)
        {
            var result = await reader.ReadAsync(cancellationToken);
            var buffer = result.Buffer;

            while (TryReadLine(ref buffer, out var jsonData))
                containers.Add(DeserializeJsonData<T>(jsonData));

            reader.AdvanceTo(buffer.Start, buffer.End);

            if (!result.IsCompleted)
                continue;

            if (buffer.Length > 0 && buffer.FirstSpan[0] != (byte)'\n')
                containers.Add(DeserializeJsonData<T>(buffer));

            break;
        }

        await reader.CompleteAsync();
        return containers;

        static bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            var reader = new SequenceReader<byte>(buffer);
            while (!reader.End)
            {
                if (reader.TryReadToAny(out ReadOnlySequence<byte> sequence, "\r\n"u8))
                {
                    if (sequence.Length == 0)
                        continue;

                    buffer = buffer.Slice(reader.Position);

                    line = sequence;
                    return true;
                }

                reader.Advance(buffer.Length - reader.Consumed);
            }

            line = default;
            return false;
        }

        static TData DeserializeJsonData<TData>(ReadOnlySequence<byte> jsonData)
        {
            var jsonReader = new Utf8JsonReader(jsonData);
            return JsonSerializer.Deserialize<TData>(ref jsonReader)
                ?? throw new InvalidOperationException();
        }
    }
}
