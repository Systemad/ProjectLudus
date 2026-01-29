using System.IO;

namespace AppHost.Extensions;

public static class PgSearchExtension
{
    public static string USER = "becknova";
    public static string POSTGRES_VERSION = "17";
    public static string PG_SEARCH_TAG = $"v0.19.4-pg{POSTGRES_VERSION}";
    public static string BASE_IMAGE = $"paradedb/paradedb:{PG_SEARCH_TAG}";
    public static string IMAGE_TAG = $"{USER}/paradedb-ext:{PG_SEARCH_TAG}";
    public static string PLATFORMS = $"linux/amd64,linux/arm64";
    public static string PGMQ_VERSION = "1.7.0";

    
    private static void EnsureDockerFileExist(string filePath)
    {
        if (!File.Exists(filePath))
        {
            var content = $"""
                FROM ${BASE_IMAGE} AS BASE
                WORKDIR /tmp
                RUN apt-get update && \
                    apt-get install -y --no-install-recommends wget make unzip && \
                    rm -rf /var/lib/apt/lists/*
                WORKDIR /tmp
                RUN wget -O pgmq-{PGMQ_VERSION}.zip https://api.pgxn.org/dist/pgmq/{PGMQ_VERSION}/pgmq-{PGMQ_VERSION}.zip && \
                    unzip pgmq-{PGMQ_VERSION}.zip
                WORKDIR /tmp/pgmq-{PGMQ_VERSION}
                RUN make && make install
                WORKDIR /
                RUN apt-get purge -y wget make unzip && \
                    apt-get autoremove -y && \
                    rm -rf /tmp/pgmq-{PGMQ_VERSION} && \
                    rm -rf /var/lib/apt/lists/*
                """;

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, content);
        }
    }
}
