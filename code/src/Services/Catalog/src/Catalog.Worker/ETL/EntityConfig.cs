using Catalog.Worker.ETL.Configs;
using IGDB.Models;

namespace Catalog.Worker.ETL;

public static class EntityConfig
{
    private static readonly Dictionary<Type, object> Configurations = new()
    {
        { typeof(Company), CompanyConfig.Config },
    };
    
    public static EntityMetadata<T> Get<T>()
    {
        if (Configurations.TryGetValue(typeof(T), out var config))
        {
            return (EntityMetadata<T>)config;
        }
        throw new KeyNotFoundException($"Configuration not found for entity type {typeof(T).Name}");
    }
}
