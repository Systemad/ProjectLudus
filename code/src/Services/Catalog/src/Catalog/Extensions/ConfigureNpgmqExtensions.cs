using Microsoft.Extensions.DependencyInjection;
using Npgmq;

namespace Catalog.Extensions;

public static class ConfigureNpgmqExtensions
{
    public static void AddNpgmq(this IServiceCollection serviceCollection, string connectionString)
    {
        var npgmqClient = new NpgmqClient("catalog-db");
        serviceCollection.AddSingleton<NpgmqClient>(npgmqClient);
    }
}